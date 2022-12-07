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
            var a = GetLargestDirectoryToDelete(parsedFileSystem, 70_000_000, 30_000_000);

            return a;
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
            return GetLargestDirectoryToDelete(parsedFileSystem, 70_000_000, 30_000_000);
        }

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

        private int ReadDirectoryListing(int startPosition, List<string> commandsAndOutput, Directory parentDirectory)
        {
            var directoryListingEnded = false;
            var seekPosition = startPosition;

            while (!directoryListingEnded)
            {
                var currLine = commandsAndOutput[seekPosition];
                if (currLine[0] == '$')
                {
                    directoryListingEnded = true;
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

                if (seekPosition >= commandsAndOutput.Count)
                {
                    directoryListingEnded = true;
                }
            }
            return seekPosition;
        }

        private Directory ParseDirectory(ReadOnlySpan<char> directoryLine)
        {
            // A directory is in the form of the following, with the directory name starting 4 characters in
            //  dir [directory name]
            //  ....^
            // So the rest of the span has to be the directory name
            var directory = new Directory(directoryLine[4..].ToString());

            return directory;
        }

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

        private long GetLargestDirectoryToDelete(Directory rootDirectory, long totalDiskSpaceAvailable, long diskSpaceRequired)
        {
            var unusedSpace = totalDiskSpaceAvailable - rootDirectory.Size;

            var candidateList = new List<Directory>();

            var directories = FlattenDirectories(rootDirectory).OrderByDescending(x => x.Size).Skip(1);
            foreach (var directory in directories)
            {
                var a = unusedSpace + directory.Size;
                if (a >= diskSpaceRequired)
                {
                    candidateList.Add(directory);
                }
            }

            return candidateList.Min(x => x.Size);
        }

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
