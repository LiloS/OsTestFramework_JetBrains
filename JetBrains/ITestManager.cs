namespace JetBrains
{
    interface ITestManager
    {
        /// <summary>
        /// Copies test assembly from host to guest.
        /// </summary>
        void CopyFilesToGuest();

        /// <summary>
        /// Runs NUnit test on guest.
        /// </summary>
        void RunTests();

        /// <summary>
        /// Copies NUnit xml output from guest to host.
        /// </summary>
        void CopyFilesToHost();

        /// <summary>
        /// Clears guest.
        /// </summary>
        void Clear();

        /// <summary>
        /// Finalizes tests run by clearing resources.
        /// </summary>
        void FinalizeTestsRun();
    }
}
