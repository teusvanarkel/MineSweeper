﻿namespace MineSweeperLogic;
public class Board
{
    private  int Width { get; set; }
    private  int Height { get; set; }
    private  int MineCount { get; set; }
    public Cell[,] Cells { get; set; }  
    private List<Mine> Mines { get; set; } = new List<Mine>();
    
    public event EventHandler BoardMineClickedEvent;
    
    public Board(int width, int height, int mineCount)
    {
        DetermineMineCells(mineCount, width, height);
        Width = width;
        Height = height;
        MineCount = mineCount;
        
        Cells = new Cell[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                MineState mineState = MineState.Empty;
                if( Mines.Where( a => a.X == x && a.Y == y).Count() > 0)
                {
                    mineState = MineState.Mine;
                }

                var cell = new Cell(){ MineState = mineState};
                cell.MineClicked+= MineClicked;
                Cells[x, y] = cell;
            }
        }
        DetermineMineBorders();
    }

    public void DetermineMineBorders()
    {
        for (int x = 0; x < Cells.GetLength(0); x++)
        {
            for (int y = 0; y < Cells.GetLength(1); y++)
            {
                if(Cells[x,y].IsMine)
                {
                    var startX = x-1;
                    var endX = startX + 3;
                    for (int i = startX; i < endX; i++)
                    {
                        var startY = y-1;
                        var endY = startY + 3;
                        for (int z = startY; z < endY; z++)
                        {
                            var foundCell = Cells[i,z];
                           if(foundCell.MineState != MineState.Mine)
                           {
                                foundCell.MineState = MineState.BordersMine;
                                foundCell.Value+=1;
                           }
                        }
                    }
                }
            }
        }
    }
    public void LeftClicked(int x, int y)
    {
        var startX = x-1;
        var endX = startX + 3;
        for (int i = startX; i < endX; i++)
        {
            var startY = y-1;
            var endY = startY + 3;
            for (int z = startY; z < endY; z++)
            {
                var foundCell = Cells[i,z ];
                if(z == y && i == x )
                {
                    foundCell.LeftClick();
                    break;
                }
                if(foundCell.MineState == MineState.Empty)
                {
                    foundCell.Reveal();
                    LeftClicked(i,z);
                }
            }
        }
    }

    public void RightClicked(int x, int y)
    {
        Cells[x,y].RightClick();
    }

    public void MineClicked(object sender, EventArgs e)
    {
        BoardMineClickedEvent.Invoke(sender,e);
    }

    private void DetermineMineCells(int mineCount, int width, int height)
    {
        for (int i = 0; i < mineCount; i++)
        {
            var x = new Random().Next(width);
            var y = new Random().Next(height);
            Mines.Add(new Mine(){ X = x, Y=y});
        }
    }
}

