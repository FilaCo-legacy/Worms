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
    /// Абстрактный класс представляющий собой некоторый объект, занимающий прямоугольник заданного размера на экране
    /// </summary>
    public abstract class Block
    {
        /// <summary>
        /// Константа, обозначающая пустое пространство на экране (пустая ячейка)
        /// </summary>
        public const int voidValue = 0;
        /// <summary>
        /// Константа, обозначающая ячейки игрока
        /// </summary>
        public const int playerValue = 1;
        /// <summary>
        /// Константа, обозначающая ячейки поверхности (земли)
        /// </summary>
        public const int groundValue = 2;
        /// <summary>
        /// Значение, которым текущий объект заполняет свою подматрицу
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// Игровое окно, на котором находится объект
        /// </summary>
        public PlayWindow Owner { get; }
        /// <summary>
        /// Координата левого верхнего угла объекта
        /// </summary>
        public Point LeftUpper;
        /// <summary>
        /// Ширина объекта
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Высота объекта
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Процедура заполнения ячеек подматрицы объекта его значением
        /// </summary>
        public void FillingMatr()
        {
            for (int i = this.LeftUpper.Y; i < this.LeftUpper.Y + this.Height; i++)
                for (int j = this.LeftUpper.X; j < this.LeftUpper.X + this.Width; j++)
                    Owner.Field[i, j] = Value;
        }
        /// <summary>
        /// Процедура очистки ячеек подматрицы объекта (заполнение значением "пустоты")
        /// </summary>
        public void ClearMatr()
        {
            for (int i = this.LeftUpper.Y; i < this.LeftUpper.Y + this.Height; i++)
                for (int j = this.LeftUpper.X; j < this.LeftUpper.X + this.Width; j++)
                    Owner.Field[i, j] = voidValue;
        }
        /// <summary>
        /// Инициализирует новый экземпляр объекта
        /// </summary>
        /// <param name="begPoint">Координата левого верхнего угла</param>
        /// <param name="ownerValue">Окно типа PlayWindow</param>
        /// <param name="widthValue">Ширина нового объекта (по умолчанию 1)</param>
        /// <param name="HeightValue">Высота нового объекта (по умолчанию 1)</param>
        public Block(Point begPoint, PlayWindow ownerValue, int widthValue = 1, int HeightValue = 1)
        {
            Owner = ownerValue;
            LeftUpper = begPoint;
            Width = widthValue;
            Height = HeightValue;
            FillingMatr();
        }
        /// <summary>
        /// Неявное преобразование в System.Drawing.Rectangle 
        /// Координаты левого верхнего угла объекта - координаты нового Rectangle
        /// Размеры объекта - размеры нового Rectangle
        /// </summary>
        /// <param name="curBlock">Объект для преобразования</param>
        public static implicit operator Rectangle(Block curBlock)
        {
            return new Rectangle(curBlock.LeftUpper.X, curBlock.LeftUpper.Y, curBlock.Width, curBlock.Height);
        }
}
}
