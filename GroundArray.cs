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
    /// Класс, осуществляющий процесс терраформирования и хранящий коллекцию объектов типа Ground
    /// </summary>
    public class GroundArray
    {
        /// <summary>
        /// Размеры исходной матрицы по координате Y
        /// </summary>
        int maxValueY = 0;
        /// <summary>
        /// Размеры исходной матрицы по координате X
        /// </summary>
        int maxValueX = 0;
        /// <summary>
        /// Максимальная высота объекта типа Ground в ландшафте
        /// </summary>
        int maxHeight = 0;
        /// <summary>
        /// Минимальная высота объекта типа Ground в ландшафте
        /// </summary>
        int minHeight = 0;
        /// <summary>
        /// Степень сглаживания ландшафта
        /// </summary>
        int iterSmooth = 15;
        /// <summary>
        /// Количество элементов типа Ground в коллекции
        /// </summary>
        public int Length { get { return Collection.Length; } }
        /// <summary>
        /// Окно, которому принадлежит создаваемый ландшафт
        /// </summary>
        private PlayWindow _owner;
        /// <summary>
        /// Свойство, предоставляющее доступ для чтения окна-хозяина
        /// </summary>
        public PlayWindow Owner
        {
            get { return _owner; }
        }
        /// <summary>
        /// Генератор случайных чисел - для формирования объектов типа Ground динамической высоты
        /// </summary>
        static Random rnd = new Random();
        /// <summary>
        /// Индексатор для доступа к элементам коллекции
        /// </summary>
        /// <param name="ind">Индекс элемента в коллекции</param>
        /// <returns>Объект типа Ground, имеющий заданный индекс в коллекции GroundArray</returns>
        public Ground this[int ind]
        {
            get
            {
                try
                {
                    return Collection[ind];
                }
                catch (Exception e)
                {
                    MessageBox.Show(this.Owner, e.Message);
                    return null;
                }
            }
        }
        /// <summary>
        /// Массив объектов типа Ground
        /// </summary>
        private Ground[] _collection;
        /// <summary>
        /// Свойство, обеспечивающе коллекции доступ "только для чтения"
        /// </summary>
        public Ground[] Collection { get { return _collection; } }
        /// <summary>
        /// Процедура сглаживания ландшафта путём взятия среднего арифметического от двух соседних значений
        /// </summary>
        public void Smoothing()
        {
            for (int i = 0; i < iterSmooth; i++)
            {
                for (int j = 1; j < Collection.Length - 1; j++)
                {
                    Collection[j].ClearMatr();
                    Collection[j].Height = (Collection[j - 1].Height + Collection[j + 1].Height) / 2;
                    Collection[j].LeftUpper.Y = maxValueY - Collection[j].Height - 1;
                    Collection[j].FillingMatr();
                }
            }
        }
        /// <summary>
        /// Процедура терраформирования.
        /// Первый и последний элементы имеют высоту 0 для закрытия кривой и корректной отрисовки
        /// </summary>
        private void TerraForm()
        {
            // Шаг изменения высоты
            int stepHeight = maxHeight / minHeight;
            // Особое значение для 1-ого элемента коллекции
            Collection[0] = new Ground(new Point(0, maxValueY - 1), Owner, 0);
            // Цикл непосредственного задания высот
            for (int i = 1; i < Collection.Length - 1; i++)
            {
                int curHeight = Collection[i - 1].Height + rnd.Next(-stepHeight, stepHeight + 1);
                if (curHeight < minHeight || curHeight > maxHeight)
                    curHeight = rnd.Next(minHeight, maxHeight + 1);
                Collection[i] = new Ground(new Point(i, maxValueY - curHeight - 1), Owner, curHeight);
            }
            // Особое значние для последнего элемента коллекции
            Collection[Length - 1] = new Ground(new Point(Length - 1, maxValueY - 1), Owner, 0);
            // Сглаживание ландшафта
            Smoothing();
        }
        /// <summary>
        /// Инициализрует объект класса GroundArray на заданном окне типа PlayWindow
        /// </summary>
        /// <param name="ownerValue">Окно типа PlayWindow, на котором будет располагаться ландшафт</param>
        public GroundArray(PlayWindow ownerValue)
        {
            _owner = ownerValue;
            maxValueX = Owner.Field.GetLength(1);
            maxValueY = Owner.Field.GetLength(0);
            maxHeight = 2 * maxValueY / 3;
            minHeight = maxValueY / 10;
            _collection = new Ground[maxValueX];
            TerraForm();
        }
        /// <summary>
        /// Неявное преобразование в массив Point.
        /// Создаётся массив, состоящий из координат левых верхних углов объектов Ground коллекции
        /// </summary>
        /// <param name="cur">Исходная коллекция</param>
        public static implicit operator Point[](GroundArray cur)
        {
            Point[] arr = new Point[cur.Length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = cur[i].LeftUpper;
            return arr;
        }
        /// <summary>
        /// Отрисовка ландшафта на форме
        /// </summary>
        public void Show()
        {
            Graphics g = Owner.CreateGraphics();
            g.FillClosedCurve(new Pen(Color.Green).Brush, this);
            g.Dispose();
        }
    }
}
