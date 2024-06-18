using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace SalaryApp.Models
{
    public class RedDiamondDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float width = dirtyRect.Width;
            float height = dirtyRect.Height;
            float centerX = width / 2;
            float centerY = height / 2;

            // Рисуем красный ромб
            canvas.FillColor = Colors.Red;
            PathF diamondPath = new PathF();
            diamondPath.MoveTo(centerX, 0);
            diamondPath.LineTo(width, centerY);
            diamondPath.LineTo(centerX, height);
            diamondPath.LineTo(0, centerY);
            diamondPath.Close();

            canvas.FillPath(diamondPath);

            // Рисуем восклицательный знак
            canvas.FillColor = Colors.White;
            float exclamationHeight = height / 2;
            float exclamationWidth = width / 8;
            canvas.FillRectangle(centerX - exclamationWidth / 2, centerY - exclamationHeight / 2, exclamationWidth, exclamationHeight);
            canvas.FillCircle(centerX, centerY + exclamationHeight / 2 + exclamationWidth, exclamationWidth / 2);
        }
    }
}
