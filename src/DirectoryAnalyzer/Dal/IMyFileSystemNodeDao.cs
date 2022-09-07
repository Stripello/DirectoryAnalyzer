namespace DirectoryAnalyzer.Dal
{
    public interface IMyFileSystemNodeDao
    {
        void Add(IList<MyFileSystemNode> node);
        /// <summary>
        /// Scanning db for stored MyFileSystemNode for given directories
        /// </summary>
        /// <param name="directoriesToSearch"></param>
        /// <returns></returns>
        IList<MyFileSystemNode> Read(IList<string> directoriesToSearch);
        /// <summary>
        /// Updating already stored elements in DB.
        /// </summary>
        void UpdateDb(IList<MyFileSystemNode> nodesToUpdate);
    }
}