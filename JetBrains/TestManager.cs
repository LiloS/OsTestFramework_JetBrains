namespace JetBrains
{
    using System.Configuration;
    using OsTestFramework;

    using System;
    using System.IO;

    public class TestManager : ITestManager
    {
        private readonly string pathToTests;
        private readonly string guestPathToTests;
        private readonly string testDllName;
        private readonly string testXmlName;
        private readonly string pathToTestsResult;
        private readonly RemoteEnvironment operatingSystem;

        public TestManager()
        {
            var ip = ConfigurationManager.AppSettings.Get("IP");
            var userName = ConfigurationManager.AppSettings.Get("Username");
            var password = ConfigurationManager.AppSettings.Get("Password");
            var pathToPsExec = ConfigurationManager.AppSettings.Get("PathToPsExec");

            pathToTests = ConfigurationManager.AppSettings.Get("PathToTests");
            guestPathToTests = ConfigurationManager.AppSettings.Get("GuestPathToTests");
            testDllName = ConfigurationManager.AppSettings.Get("TestDllName");
            testXmlName = ConfigurationManager.AppSettings.Get("TestXmlName");
            pathToTestsResult = ConfigurationManager.AppSettings.Get("PathToTestsResult");

            Guard.ArgumentNotNullOrEmpty(ip, nameof(ip));
            Guard.ArgumentNotNullOrEmpty(userName, nameof(userName));
            Guard.ArgumentNotNullOrEmpty(password, nameof(password));
            Guard.ArgumentNotNullOrEmpty(pathToPsExec, nameof(pathToPsExec));
            Guard.ArgumentNotNullOrEmpty(pathToTests, nameof(pathToTests));
            Guard.ArgumentNotNullOrEmpty(guestPathToTests, nameof(guestPathToTests));
            Guard.ArgumentNotNullOrEmpty(testDllName, nameof(testDllName));
            Guard.ArgumentNotNullOrEmpty(testXmlName, nameof(testXmlName));
            Guard.ArgumentNotNullOrEmpty(pathToTestsResult, nameof(pathToTestsResult));

            operatingSystem = new RemoteEnvironment(ip, userName, password, pathToPsExec);

            Guard.ArgumentNotNull(operatingSystem, nameof(operatingSystem));
        }

        /// <inheritdoc/>
        public void CopyFilesToGuest()
        {
            var files = Directory.GetFiles(pathToTests);

            foreach (var file in files)
            {
                operatingSystem.CopyFileFromHostToGuest(file, Path.Combine(guestPathToTests, Path.GetFileName(file)));
            }
        }

        /// <inheritdoc/>
        public void RunTests()
        {
            operatingSystem.WindowsShellInstance.ExecuteElevatedCommandInGuestNoRemoteOutput(
                $@"nunit3-console {Path.Combine(guestPathToTests, testDllName)} --result={Path.Combine(guestPathToTests, testXmlName)}",
                TimeSpan.FromSeconds(10));
        }

        /// <inheritdoc/>
        public void CopyFilesToHost()
        {
            operatingSystem.CopyFileFromGuestToHost(Path.Combine(guestPathToTests, testXmlName), Path.Combine(pathToTestsResult, testXmlName));
        }

        /// <inheritdoc/>
        public void Clear()
        {
            if (operatingSystem.DirectoryExistsInGuest(guestPathToTests))
            {
                operatingSystem.WindowsShellInstance.ExecuteElevatedCommandInGuest(
                    $@"rmdir /s /q {guestPathToTests}",
                    TimeSpan.FromSeconds(10));
            }
        }

        /// <inheritdoc/>
        public void FinalizeTestsRun()
        {
            this.Clear();
            operatingSystem?.Dispose();
        }
    }
}
