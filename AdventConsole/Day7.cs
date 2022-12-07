using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdventConsole
{
    internal class Day7 : BaseDay<long>
    {
        public override long Part1Test()
        {
            var input = new List<string>
            {
               "$ cd /",
               "$ ls",
               "dir a",
               "14848514 b.txt",
               "8504156 c.dat",
               "dir d",
               "$ cd a",
               "$ ls",
               "dir e",
               "29116 f",
               "2557 g",
               "62596 h.lst",
               "$ cd e",
               "$ ls",
               "584 i",
               "$ cd ..",
               "$ cd ..",
               "$ cd d",
               "$ ls",
               "4060174 j",
               "8033020 d.log",
               "5626152 d.ext",
               "7214296 k",
            };

            var parsedFileSystem = ParseFileSystem(input);

            return GetDirectoriesUnderSize(parsedFileSystem, 100_000)
                .Select(x => x.Size)
                .Sum();
        }

        public override long Part2Test()
        {
            var input = new List<string>
            {
                "$ cd /",
                "$ ls",
                "dir a",
                "14848514 b.txt",
                "8504156 c.dat",
                "dir d",
                "$ cd a",
                "$ ls",
                "dir e",
                "29116 f",
                "2557 g",
                "62596 h.lst",
                "$ cd e",
                "$ ls",
                "584 i",
                "$ cd ..",
                "$ cd ..",
                "$ cd d",
                "$ ls",
                "4060174 j",
                "8033020 d.log",
                "5626152 d.ext",
                "7214296 k",
            };

            var parsedFileSystem = ParseFileSystem(input);
            var a = GetSmallestDirectoryToDelete(parsedFileSystem, 70_000_000, 30_000_000);

            return a.Size;
        }

        public override long GetPart1Answer()
        {
            var parsedFileSystem = ParseFileSystem(Input);

            return GetDirectoriesUnderSize(parsedFileSystem, 100_000)
                .Select(x => x.Size)
                .Sum();

        }

        public override long GetPart2Answer()
        {
            var parsedFileSystem = ParseFileSystem(Input);
            return GetSmallestDirectoryToDelete(parsedFileSystem, 70_000_000, 30_000_000).Size;
        }

        /// <summary>
        /// Parses the command output to build a file system, returning the root node of the directory (always "/")
        /// </summary>
        /// <param name="commandsAndOutput"></param>
        /// <returns></returns>
        private Directory ParseFileSystem(List<string> commandsAndOutput)
        {
            var root = new Directory("/");

            var position = 0;
            // Start at the root directory
            var currentDirectory = root;

            while (position != commandsAndOutput.Count)
            {
                var commandLine = commandsAndOutput[position].AsSpan();
                if (commandLine[0] == '$')
                {
                    // executed commands start with a dollar sign, which is immediately followed by a space
                    // so parse the command starting at the 2nd char
                    var command = ParseCommand(commandLine[2..]);
                    if (command == CommandType.ChangeDirectory)
                    {
                        // Change directory to the the contents of the remainder of the command line, starting at index 5
                        // $ cd [directory]
                        // .....^

                        var changeArgument = commandLine[5..];

                        if (changeArgument.Equals("..", StringComparison.Ordinal))
                        {
                            currentDirectory = currentDirectory.Parent;
                        }
                        else if (changeArgument.Equals("/", StringComparison.Ordinal))
                        {
                            currentDirectory = root;
                        }
                        else
                        {
                            var dir = commandLine[5..].ToString();
                            currentDirectory = currentDirectory.Children.First(x => x.Name == dir) as Directory;
                        }

                        position++;
                    }
                    else if (command == CommandType.List)
                    {
                        position = ReadDirectoryListing(position + 1, commandsAndOutput, currentDirectory);
                    }
                }
            }

            return root;
        }

        /// <summary>
        /// Reads and populates based on a directory listing up to the next command line, and returns the new offset position in the input to read from
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="commandsAndOutput"></param>
        /// <param name="parentDirectory"></param>
        /// <returns></returns>
        private int ReadDirectoryListing(int startPosition, List<string> commandsAndOutput, Directory parentDirectory)
        {
            var directoryListingEnded = false;
            var seekPosition = startPosition;

            while (!directoryListingEnded)
            {
                var currLine = commandsAndOutput[seekPosition];
                // Stop looping if we've reached a new command/the end of the directory list block
                if (currLine[0] == '$')
                {
                    break;
                }

                if (char.IsLetter(currLine[0]))
                {
                    var directory = ParseDirectory(currLine.AsSpan());
                    directory.Parent = parentDirectory;
                    parentDirectory.Children.Add(directory);
                }

                if (char.IsDigit(currLine[0]))
                {
                    var file = ParseFile(currLine.AsSpan());
                    parentDirectory.Children.Add(file);
                }

                seekPosition++;

                // dirty case if the next position would exceed the bounds of the input.
                // I could remove this and directoryListingEnded but for _relative_ readability
                // I leave this here so the while loop indicates another case that would end the loop,
                // even if this would only ever happen at EoF.
                // The currLine[0] == '$' is what actually breaks the loop but I'm a bad programmer and this is AoC2022
                // yee haw cowboy hats on
                if (seekPosition >= commandsAndOutput.Count)
                {
                    directoryListingEnded = true;
                }
            }
            return seekPosition;
        }

        /// <summary>
        /// Parses a directory from a span where a directory is defined: dir [A-Z]
        /// </summary>
        /// <param name="directoryLine"></param>
        /// <returns></returns>
        private Directory ParseDirectory(ReadOnlySpan<char> directoryLine)
        {
            // A directory is in the form of the following, with the directory name starting 4 characters in
            //  dir [directory name]
            //  ....^
            // So the rest of the span has to be the directory name
            var directory = new Directory(directoryLine[4..].ToString());

            return directory;
        }

        /// <summary>
        /// Parses a file size and name from a span where a file is defined: [0-9]\s[\d+]
        /// </summary>
        /// <param name="fileLine"></param>
        /// <returns></returns>
        private File ParseFile(ReadOnlySpan<char> fileLine)
        {
            // A file is in the form of the following, with the file name starting at the first alpha character
            //  134125 [File name]
            //  .......^
            // The start of the span is numeric that indicates the size of the file in bytes

            // Size and name are always split with a whitespace
            var segmentDivider = fileLine.IndexOf(" ");
            var fileSize = fileLine[..segmentDivider];
            var fileName = fileLine[(segmentDivider + 1)..];

            var file = new File(fileName.ToString(), int.Parse(fileSize));

            return file;
        }

        /// <summary>
        /// Parses a command string and returns its type
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        private CommandType ParseCommand(ReadOnlySpan<char> command)
        {
            // A command can only be 2 characters
            var commandString = command[..2];
            if (commandString.Equals("cd", StringComparison.Ordinal))
            {
                return CommandType.ChangeDirectory;
            }

            if (commandString.Equals("ls", StringComparison.Ordinal))
            {
                return CommandType.List;
            }

            throw new NotSupportedException($"'{commandString}' is not a supported command");
        }

        /// <summary>
        /// Returns the smallest directory that could be deleted that would free the required space given by <paramref name="diskSpaceRequired"/>
        /// </summary>
        /// <param name="rootDirectory"></param>
        /// <param name="totalDiskSpaceAvailable"></param>
        /// <param name="diskSpaceRequired"></param>
        /// <returns></returns>
        private Directory GetSmallestDirectoryToDelete(Directory rootDirectory, long totalDiskSpaceAvailable, long diskSpaceRequired)
        {
            var unusedSpace = totalDiskSpaceAvailable - rootDirectory.Size;

            var candidateList = new List<Directory>();

            var directories = FlattenDirectories(rootDirectory).OrderByDescending(x => x.Size).Skip(1);
            foreach (var directory in directories)
            {
                // Calculate the new unused space if we were to delete this directory.
                // which basically means increase the unused space by the directory size
                var unusedSpaceIfDeleted = unusedSpace + directory.Size;
                // If deleting the directory would meet or exceed the required size, add it to a candidate list
                if (unusedSpaceIfDeleted >= diskSpaceRequired)
                {
                    candidateList.Add(directory);
                }
            }
            // Return the smallest large directory we could delete to meet the required unused space requirement
            return candidateList.MinBy(x => x.Size);
        }

        /// <summary>
        /// Returns all directories and child directories recurssively that are equal to or less than <paramref name="threshold"/>
        /// </summary>
        /// <param name="current"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        private List<Directory> GetDirectoriesUnderSize(Directory current, long threshold)
        {
            var underThresholdDirectories = new List<Directory>();

            foreach (var child in current.Children)
            {
                if (child is Directory d)
                {
                    if (d.Size <= threshold)
                    {
                        underThresholdDirectories.Add(d);
                    }
                    underThresholdDirectories.AddRange(GetDirectoriesUnderSize(d, threshold));
                }
            }

            return underThresholdDirectories;
        }

        /// <summary>
        /// Returns all directories under the given <see cref="Directory"/> traversing all child directories
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private List<Directory> FlattenDirectories(Directory current)
        {
            var directoryList = new List<Directory>();
            directoryList.Add(current);

            foreach (var child in current.Children)
            {
                if (child is Directory d)
                {
                    if (d.Children.Count > 0)
                    {
                        directoryList.AddRange(FlattenDirectories(d));
                    }
                }
            }

            return directoryList;
        }

        private enum CommandType
        {
            ChangeDirectory,
            List
        }

        private class FileSystemObjectBase
        {
            public string Name;
            public virtual long Size { get; protected set; }

            protected FileSystemObjectBase(string name)
            {
                Name = name;
            }
        }

        private class Directory : FileSystemObjectBase
        {
            public Directory Parent { get; set; }
            public List<FileSystemObjectBase> Children = new();

            public override long Size => CalculateSize();

            public Directory(string name) : base(name) { }

            private long CalculateSize()
            {
                var accumulatedSize = 0L;
                foreach (var child in Children)
                {
                    if (child is File f)
                    {
                        accumulatedSize += f.Size;
                    }
                    else if (child is Directory d)
                    {
                        accumulatedSize += d.Size;
                    }
                }
                return accumulatedSize;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        private class File : FileSystemObjectBase
        {
            public File(string name, long size) : base(name)
            {
                Size = size;
            }
        }
    }
}
