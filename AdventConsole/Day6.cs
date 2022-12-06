using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventConsole
{
    internal class Day6 : BaseDay<int[]>
    {
        private int _markerLength = 4;
        private int _messageLength = 14;

        public override int[] Part1Test()
        {
            var input = new List<string>
            {
                "mjqjpqmgbljsphdztnvjfqwrcgsmlb",
                "bvwbjplbgvbhsrlpgdmjqwftvncz",
                "nppdvjthqldpwncqszvftbrmjlhg",
                "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg",
                "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw"
            };

            var markerOffsets = new List<int>();

            foreach (var messageInput in input)
            {
                markerOffsets.Add(ProcessLineForStartOfPacket(messageInput.AsSpan(), _markerLength));
            }

            return markerOffsets.ToArray();
        }

        public override int[] Part2Test()
        {
            var input = new List<string>
            {
                "mjqjpqmgbljsphdztnvjfqwrcgsmlb",
                "bvwbjplbgvbhsrlpgdmjqwftvncz",
                "nppdvjthqldpwncqszvftbrmjlhg",
                "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg",
                "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw"
            };

            var markerOffsets = new List<int>();

            foreach (var messageInput in input)
            {
                markerOffsets.Add(ProcessLineForStartOfPacket(messageInput.AsSpan(), _messageLength));
            }

            return markerOffsets.ToArray();
        }

        public override int[] GetPart1Answer()
        {
            return new[] { ProcessLineForStartOfPacket(Input[0].AsSpan(), _markerLength) };
        }

        public override int[] GetPart2Answer()
        {
            return new[] { ProcessLineForStartOfPacket(Input[0].AsSpan(), _messageLength) };
        }

        private int ProcessLineForStartOfPacket(ReadOnlySpan<char> lineBuffer, int messageSize)
        {
            for (var i = 0; i < lineBuffer.Length; i++)
            {
                if (BufferAllDistinct(lineBuffer[i..(i + messageSize)]))
                {
                    // If the buffer is all distinct, return the index of the start of the message
                    return i + messageSize;
                }
            }

            throw new InvalidOperationException("Message string contained no valid markers");
        }

        private bool BufferAllDistinct(ReadOnlySpan<char> buffer)
        {
            // check the buffer for any duplicate characters and return true if the buffer
            // contains no repeat characters
            for (var i = 0; i < buffer.Length; i++)
            {
                // Check the offset character in the buffer against the remaining characters in the span.
                // If it exists, the buffer is not all distinct
                if (buffer[(i + 1)..buffer.Length].Contains(buffer[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
