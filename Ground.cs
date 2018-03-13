using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Worms
{
    /// <summary>
    /// Класс, представляющий собой один блок земли
    /// По сути является прямоугольником шириной 1, заданной высотой и координатой левого верхнего угла
    /// </summary>
    public class Ground:Block
    {
        /// <summary>
        /// Ручка для отрисовки отдельных объектов типа Ground
        /// </summary>
        public static Pen groundFillPen = new Pen(Brushes.Green);
        /// <summary>
        /// Инициализирует новый объект класса Ground
        /// </summary>
        /// <param name="begPoint">Координата левого верхнего угла</param>
        /// <param name="ownerValue">Окно типа PlayWindow, в котором будет находиться новый объект</param>
        /// <param name="heightValue">Значение высоты (по умолчанию 1)</param>
        public Ground(Point begPoint, PlayWindow ownerValue, int heightValue=1)
            : base(begPoint, ownerValue, 1 ,heightValue)
        {
            Value = groundValue;
        }
        /// <summary>
        /// Отрисовка объекта класса Ground
        /// </summary>
        public void Show()
        {
            Graphics g = Owner.CreateGraphics();
            g.FillRectangle(groundFillPen.Brush, this);
            g.Dispose();
        }        
    }

}
