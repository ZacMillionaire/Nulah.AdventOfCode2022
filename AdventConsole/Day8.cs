using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AdventConsole
{
    internal class Day8 : BaseDay<int>
    {
        public override int Part1Test()
        {
            var input = new List<string>
            {
                "30373",
                "25512",
                "65332",
                "33549",
                "35390",
            };


            return GetTreesVisible(input);
        }

        public override int Part2Test()
        {
            return -1;
        }

        public override int GetPart1Answer()
        {
            return GetTreesVisible(Input);
        }

        public override int GetPart2Answer()
        {
            return -1;
        }

        private int GetTreesVisible(List<string> treeGrid)
        {
            // Offset the width by 1 so we don't need to do this offset constantly to change rows
            // and maintain 0 indexing
            var columnWidth = treeGrid[0].Length - 1;
            // the outside ring of trees is always visible, so get the trees surrounding
            // by taking the sqaure of the offset width
            var baseVisibleCount = columnWidth * columnWidth;

            var array = ToArrayLayout(treeGrid);
            return CalculateVisibility(array, columnWidth, columnWidth) + baseVisibleCount;
        }

        private int[][] ToArrayLayout(List<string> input)
        {
            return input.Select(x => x.Select(y => int.Parse(y.ToString())).ToArray()).ToArray();
        }

        private int CalculateVisibility(int[][] treeGrid, int gridHeight, int gridWidth)
        {
            var treeVisible = new List<int>();

            // Loop over all trees within the border
            for (var x = 1; x < gridWidth; x++)
            {
                for (var y = 1; y < gridHeight; y++)
                {
                    var treeToCheck = treeGrid[y][x];

                    var a = new SurroundHeights();

                    for (var index = 0; index <= gridWidth; index++)
                    {
                        var t = treeGrid[y][index];
                        if (index < x)
                        {
                            if (t > a.Left)
                            {
                                a.Left = t;
                            }
                        }
                        else if (index > x)
                        {
                            if (t > a.Right)
                            {
                                a.Right = t;
                            }
                        }
                    }

                    for (var index = 0; index <= gridHeight; index++)
                    {
                        var t = treeGrid[index][x];
                        if (index < y)
                        {
                            if (t > a.Top)
                            {
                                a.Top = t;
                            }
                        }
                        else if (index > y)
                        {
                            if (t > a.Bottom)
                            {
                                a.Bottom = t;
                            }
                        }
                    }

                    if (a.VisibleFromHeights(treeToCheck))
                    {
                        treeVisible.Add(treeToCheck);
                    }
                }
            }

            return treeVisible.Count;
        }

        private class SurroundHeights
        {
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
            public int Left { get; set; }

            public bool VisibleFromHeights(int source)
            {
                return source > Top
                       || source > Right
                       || source > Bottom
                       || source > Left;
            }
        }

        private bool CheckHorizontalVisibility(int[] row, int treeSize)
        {
            // Loop over the row for any tree larger than the comparison treeSize
            foreach (var t in row)
            {
                if (t < treeSize)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckVerticalVisibility(int[] col, int treeSize)
        {
            // Loop over the row for any tree larger than the comparison treeSize
            foreach (var t in col)
            {
                if (t < treeSize)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckSurrounds(int[][] treeGrid, int row, int col)
        {

            return false;
        }
    }
}
