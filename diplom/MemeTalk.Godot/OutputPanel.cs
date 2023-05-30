using Godot;
using System;
using System.Collections.Generic;
using Range = Godot.Range;

public partial class OutputPanel : Panel
{
    private static readonly Color X_AXIS_COLOR = new Color(1, 0.5f, 0.5f);
    private static readonly Color Y_AXIS_COLOR = new Color(0.5f, 1.0f, 0.5f);

    private Vector2 _viewScale = new Vector2(50, 50);
    private Vector2 _viewOffset = new Vector2(0, 0);
    private Color _gridColor = new Color(1, 1, 1, 0.15f);

    private Vector2 _grid_step = new Vector2(1, 1);

    private List<Func<double, double>> _functions = new()
    {
        x => x * 2,
    };

    private struct Pixel
    {
        public Vector2 Position;
        public Color Color;
    }

    private List<Pixel> _pixels = new List<Pixel>();

    public void Clear()
    {
        _pixels.Clear();
        _functions.Clear();
    }

    public void AddGraph(Func<double, double> func)
    {
        _functions.Add(func);
        QueueRedraw();
    }


    public override void _Draw()
    {
        if (_pixels.Count != 0)
        {
            DrawPixels();
        }
        else
        {
            DrawFunctions();
        }
    }

    private void DrawPixels()
    {
        DrawSetTransform(Size / 2, 0, new Vector2(1, -1));
        foreach (var pixel in _pixels)
        {
            DrawRect(new Rect2(pixel.Position, new Vector2(1, 1)), pixel.Color);
        }
    }

    private void DrawFunctions()
    {
        var step_x = 1.0f / _viewScale.X;
        var step_y = 1.0f / _viewScale.Y;

        var pixel_view_offset = new Vector2(-_viewOffset.X, _viewOffset.Y) * _viewScale;
        DrawSetTransform(Size / 2 + pixel_view_offset, 0, new Vector2(_viewScale.X, -_viewScale.Y));

        DrawGrid();

        var pixel_x_min = (int)Math.Floor(-Size.X * 0.5f - pixel_view_offset.X);
        var pixel_x_max = (int)Math.Floor(Size.X * 0.5f - pixel_view_offset.X);


        foreach (var func in _functions)
        {
            List<Vector2> points = new List<Vector2>();
            var pi = 0;
            double i = pixel_x_min;
            while (i < pixel_x_max)
            {
                i += step_x;
                var x = (float)i / _viewScale.X;
                var y = (float)func(x);

                if (IsOutsideView(GraphToPixelPosition(new Vector2(x, y))))
                {
                    continue;
                }

                if (!float.IsNaN(y) && !float.IsInfinity(y))
                {
                    points.Add(new Vector2(x, y));
                    pi++;
                }
                else
                {
                    if (pi > 1)
                    {
                        DrawPolyline(points.ToArray(), pi, Colors.Red);
                        points.Clear();
                    }

                    pi = 0;
                }
            }

            if (pi > 1)
            {
                DrawPolyline(points.ToArray(), pi, Colors.Red);
            }
        }
    }


    public void DrawPolyline(Vector2[] points, int count, Color color)
    {
        var pts = new Vector2[(count - 2) * 2 + 2];
        pts[0] = points[0];
        var j = 1;
        for (int i = 1; i < count - 1; i++)
        {
            pts[j] = points[i];
            j++;
            pts[j] = points[i];
            j++;
        }

        pts[pts.Length - 1] = points[count - 1];
        DrawPolyline(pts, color);
    }

    private bool IsOutsideView(Vector2 ppos)
    {
        return ppos.X < 0 || ppos.X > Size.X || ppos.Y < 0 || ppos.Y > Size.Y;
    }

    private Vector2 PixelToGraphPosition(Vector2 ppos)
    {
        return (new Vector2(ppos.X, Size.Y - ppos.Y) - Size / 2) / _viewScale + _viewOffset;
    }

    private Vector2 GraphToPixelPosition(Vector2 gpos)
    {
        return (gpos - _viewOffset) * _viewScale + Size / 2;
    }

    public void DrawGrid()
    {
        var step = _grid_step;
        var gmin = PixelToGraphPosition(new Vector2(0, Size.Y)).Snapped(step);
        var gmax = PixelToGraphPosition(new Vector2(Size.X, 0)).Snapped(step);

        var counts = Size / (_viewScale * step);
        var max_counts = Size / 2;

        if (counts.X < max_counts.X)
        {
            var x = gmin.X;
            while (x < gmax.X)
            {
                DrawLine(new Vector2(x, gmin.Y), new Vector2(x, gmax.Y), _gridColor);
                x += step.X;
            }
        }

        if (counts.Y < max_counts.Y)
        {
            var y = gmin.Y;
            while (y < gmax.Y)
            {
                DrawLine(new Vector2(gmin.X, y), new Vector2(gmax.X, y), _gridColor);
                y += step.Y;
            }
        }

        DrawLine(new Vector2(gmin.X, 0), new Vector2(gmax.X, 0), X_AXIS_COLOR);
        DrawLine(new Vector2(0, gmin.Y), new Vector2(0, gmax.Y), Y_AXIS_COLOR);
    }

    public void AddPixel(Vector2 arg1, Color arg2)
    {
        _functions.Clear();
        _pixels.Add(new Pixel { Position = arg1, Color = arg2 });
        QueueRedraw();
    }
}