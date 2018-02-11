namespace JetBrains
{
    using System;
    using Unity;

    class Program
    {
        static void Main(string[] args)
        {
            // Register mappings for DI.
            var container = new UnityContainer();
            container.RegisterType<ITestManager, TestManager>();

            // Resolve mapping.
            var testManager = container.Resolve<ITestManager>();

            // Run test.
            testManager.Clear();

            testManager.CopyFilesToGuest();
            testManager.RunTests();
            testManager.CopyFilesToHost();

            testManager.FinalizeTestsRun();

            Console.WriteLine("------------------FINISHED------------------");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
