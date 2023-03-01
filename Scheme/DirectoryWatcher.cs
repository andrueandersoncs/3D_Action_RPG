using System.IO;
using UnityEngine;

namespace Player
{
    /*
     * New scheme scripts can be added at runtime by adding them to the Assets/Scripts/Scheme folder.
     * This class handles watching the directory, loading, and interpreting the new *.ss files.
     * Only one of this class is needed, just like the SchemyInterpreter class.
     */
    public class DirectoryWatcher : MonoBehaviour
    {
        private static DirectoryWatcher instance;
        
        private void OnEnable()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }
            
            instance = this;
            
            WatchDirectory("./Assets/Scripts/Scheme");
        }

        private static void WatchDirectory(string directoryPath)
        {
            // Create a new FileSystemWatcher and set its properties
            var watcher = new FileSystemWatcher();
            watcher.Path = directoryPath;
            watcher.IncludeSubdirectories = true;
            watcher.Filter = "*.ss";

            // Watch for changes to the files in the directory
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Register event handlers for the various file system events
            watcher.Created += OnFileCreated;
            watcher.Changed += OnFileChanged;
            watcher.Deleted += OnFileDeleted;
            watcher.Renamed += OnFileRenamed;

            // Start watching the directory
            watcher.EnableRaisingEvents = true;
        }

        // Event handler for file created
        private static void OnFileCreated(object source, FileSystemEventArgs e)
        {
            Debug.Log("File created: " + e.FullPath);
        }

        // Event handler for file changed
        private static void OnFileChanged(object source, FileSystemEventArgs e)
        {
            Debug.Log("File changed: " + e.FullPath);
            
        }

        // Event handler for file deleted
        private static void OnFileDeleted(object source, FileSystemEventArgs e)
        {
            Debug.Log("File deleted: " + e.FullPath);
        }

        // Event handler for file renamed
        private static void OnFileRenamed(object source, RenamedEventArgs e)
        {
            Debug.Log($"File renamed: {e.OldFullPath} to {e.FullPath}");
        }

    }
}