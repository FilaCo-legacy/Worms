using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Worms
{
    /// <summary>
    /// Класс, описывающий игрока и его поведение относительно других объектов
    /// </summary>
    public class Player : Block
    {
        /// <summary>
        /// Массив спрайтов для различных состояний игрока.
        /// 0. Начальнный этап движения
        /// 1. Промежуточный этап
        /// 2. Стандартное состояние
        /// 3. Сменное стандартное состояние (перемена между 2 и 3 симулирует "дыхание" игрока)
        /// 4. "Полётное" состояние
        /// </summary>
        static Image[] PlayerVid = { Image.FromFile(@"D:\пользователь\Desktop\Курсовая\Червяк3.png"),
            Image.FromFile(@"D:\пользователь\Desktop\Курсовая\Червяк2.png"),
            Image.FromFile(@"D:\пользователь\Desktop\Курсовая\Червяк1.png"),
        Image.FromFile(@"D:\пользователь\Desktop\Курсовая\Червяк4.png"),
        Image.FromFile(@"D:\пользователь\Desktop\Курсовая\Червяк5.png") };
        /// <summary>
        /// Флаг, который показывает движется игрок или нет
        /// </summary>
        public bool IsMoving = false;
        /// <summary>
        /// Поверхность, на которой находятся игроки
        /// </summary>
        private GroundArray Surface;
        private const int maxWidth = 50;
        /// <summary>
        /// Текущее изображение червяка (изначально стандартное состояние)
        /// </summary>
        private Bitmap curImage = new Bitmap(PlayerVid[2]);
        /// <summary>
        /// Константа, хранящая максимальную скорость игрока при движении
        /// </summary>
        private const int maxStep = 3;
        /// <summary>
        /// Константа, хранящая максимальную скорость игрока при полёте
        /// </summary>
        private const int maxSpeed = 30;
        /// <summary>
        /// Константа, хранящая "проходимость" червя
        /// </summary>
        private const int patency = 10;
        /// <summary>
        /// Константа перерисовки поверхности под игроком
        /// </summary>
        private const int distantSurfaceReDraw = 10;
        /// <summary>
        /// Шаг "кручения" спрайта при прыжке назад
        /// </summary>
        private int roll = 0;
        /// <summary>
        /// "Состояния" игрока: 0 - Игрок стоит на месте; 1 - Игрок движется; 2 - Игрок падает; 3 - Игрок делает сальто
        /// </summary>
        public int state = 0;
        /// <summary>
        /// Текущий шаг игрока при движении
        /// </summary>
        private int step = 0;
        /// <summary>
        /// Направление игрока: 0 - Игрок смотрит влево; 1 - Игрок смотрит вправо
        /// </summary>
        private int dir = 0;
        /// <summary>
        /// Разница между размером хитбокса и самой широкой отображаемой картинкой пополам.
        /// Нужно для того, чтобы поместить хитбокс по центру отображаемой картинки
        /// </summary>
        private const int difPic = 5;
        /// <summary>
        /// Количество игроков на карте
        /// </summary>
        public static int Count { get; set; }
        /// <summary>
        /// Инициализирует объект класса Player
        /// </summary>
        /// <param name="surfaceValue">Ландшафт, на котором находится игрок</param>
        /// <param name="begPoint">Координата верхнего левого угла у подматрицы игрока</param>
        /// <param name="ownerValue">Окно, в котором отображается игрок</param>
        /// <param name="widthValue">Ширина подматрицы игрока</param>
        /// <param name="heightValue">Высота подматрицы игрока</param>
        public Player(Point begPoint, PlayWindow ownerValue,
            int widthValue = PlayWindow.playerWidth, int heightValue = PlayWindow.playerHeight)
            : base(begPoint, ownerValue, widthValue, heightValue)
        {
            Count++;
            Value = playerValue;
            Surface = Owner.Surface;

        }
        /// <summary>
        /// Процедура отображения спрайта игрока по заданной траектории
        /// </summary>
        /// <param name="trajectory">Траектория движения</param>
        public void Visioning(Point[] trajectory)
        {
            foreach (Point p in trajectory)
                Show(p);
        }
        /// <summary>
        /// Промежуточная процедура для зарисовки спрайта игрока изображением под ним
        /// </summary>
        /// <param name="location">Текущая позиция спрайта</param>
        private void Hide(Point location)
        {
            Graphics g = Owner.CreateGraphics();
            //Берётся фрагмент заднепланового изображения с формы в текущей позиции
            Bitmap curPref = Owner.prefImage.Clone(new Rectangle(location, curImage.Size),
                System.Drawing.Imaging.PixelFormat.DontCare);
            // Отрисовывается "подкладка"
            g.DrawImage(curPref, location);
            // Перерисовывается область поверхности под игроком
            for (int i = Math.Max(0, location.X - distantSurfaceReDraw);
                i < Math.Min(location.X + curImage.Width + distantSurfaceReDraw - 1, Owner.Field.GetLength(1)); i++)
                Surface[i].Show();
            g.Dispose();
        }
        /// <summary>
        /// Процедура отображения спрайта игрока на указанном месте
        /// </summary>
        /// <param name="location"></param>
        public void Show(Point location)
        {
            Graphics g = Owner.CreateGraphics();
            //g.DrawRectangle(new Pen(Color.Black), this);
            switch (state)
            {
                case 0:
                    curImage = new Bitmap(PlayerVid[2]);
                    if (dir == 1)
                        curImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    g.DrawImage(curImage, location.X - difPic, location.Y);
                    Thread.Sleep(500);
                    this.Hide(new Point(location.X - difPic, location.Y));
                    curImage = new Bitmap(PlayerVid[3]);
                    if (dir == 1)
                        curImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    g.DrawImage(curImage, location.X - difPic, location.Y);
                    Thread.Sleep(500);
                    this.Hide(new Point(location.X - difPic, location.Y));
                    break;
                case 1:
                    curImage = new Bitmap(PlayerVid[step % 3]);
                    if (dir == 1)
                        curImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    g.DrawImage(curImage, location.X - difPic, location.Y);
                    Thread.Sleep(75);
                    this.Hide(new Point(location.X - difPic, location.Y));
                    break;
                case 2:
                    curImage = new Bitmap(PlayerVid[4]);
                    if (dir == 1)
                        curImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    g.DrawImage(curImage, location);
                    Thread.Sleep(75);
                    this.Hide(location);
                    break;
                case 3:
                    curImage = new Bitmap(PlayerVid[4]);
                    if (dir == 1)
                    {
                        curImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        switch (roll)
                        {
                            case 0:
                                curImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                roll++;
                                break;
                            case 1:
                                curImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                roll++;
                                break;
                            case 2:
                                curImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                roll++;
                                break;
                            case 3:
                                state = 2;
                                roll = 0;
                                break;

                        }
                    }
                    else
                    {
                        switch (roll)
                        {
                            case 0:
                                curImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                roll++;
                                break;
                            case 1:
                                curImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                roll++;
                                break;
                            case 2:
                                curImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                roll++;
                                break;
                            case 3:
                                state = 2;
                                roll = 0;
                                break;
                        }
                    }
                    g.DrawImage(curImage, location);
                    Thread.Sleep(75);
                    this.Hide(location);
                    break;
                case 4:
                    curImage = new Bitmap(PlayerVid[2]);
                    if (dir == 1)
                        curImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    g.DrawImage(curImage, location.X - difPic, location.Y);
                    break;
            }
            g.Dispose();
        }
        /// <summary>
        /// Функция проверки пересечения подматриц объектов
        /// </summary>
        /// <returns></returns>
        private bool CheckSubField()
        {
            for (int i = this.LeftUpper.Y; i < this.LeftUpper.Y + this.Height; i++)
                for (int j = this.LeftUpper.X; j < this.LeftUpper.X + this.Width; j++)
                    if (Owner.Field[i, j] != 0)
                        return false;
            return true;
        }
        /// <summary>
        /// Функция проверки возможности прохода игрока через препятствие
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private bool CheckPatency(int column)
        {
            int curDif = 0;
            while (Owner.Field[LeftUpper.Y + Height - curDif, column] != 0)
                curDif++;
            return curDif <= patency;
        }
        /// <summary>
        /// Функция возвращающая расстояние до земли от нижней границы хитбокса игрока
        /// </summary>
        /// <returns></returns>
        private int DistanceToGround()
        {
            int curY = LeftUpper.Y;
            while (!PlayerStand())
                LeftUpper.Y++;
            int dist = LeftUpper.Y - curY;
            LeftUpper.Y -= dist;
            return dist;
        }
        /// <summary>
        /// Функция обеспечивающая шаг игрока
        /// </summary>
        /// <param name="direction">Направление: 0 - Игрок смотрит влево; 1 - Игрок смотрит вправо</param>
        public void Move(int direction)
        {
            // Если игрок не на поверхности
            if (!PlayerStand())
                return;
            IsMoving = true;
            // Переход в состояние ходьбы
            state = 1;
            // Переменная дистанции до земли
            int dist = 0;
            // Точки траектории движения
            List<Point> trajectory = new List<Point>(maxStep);
            switch (direction)
            {
                case 0:
                    if (LeftUpper.X - difPic == 0)
                        return;
                    ClearMatr();
                    LeftUpper.X -= maxStep;
                    while (LeftUpper.X - difPic < 0 || !CheckPatency(LeftUpper.X))
                        LeftUpper.X++;
                    if (!CheckSubField() && CheckPatency(LeftUpper.X))
                        LeftUpper.Y -= (LeftUpper.Y - Surface[LeftUpper.X].LeftUpper.Y + Height + 1);
                    dist = DistanceToGround();
                    if (dist <= patency)
                        LeftUpper.Y += dist;
                    trajectory.Add(LeftUpper);

                    break;
                case 1:
                    if (LeftUpper.X + curImage.Width - difPic >= Owner.Field.GetLength(1))
                        return;
                    ClearMatr();
                    LeftUpper.X += maxStep;
                    while (LeftUpper.X + curImage.Width - difPic >= Owner.Field.GetLength(1) ||
                        !CheckPatency(LeftUpper.X + Width - 1))
                        LeftUpper.X--;
                    if (!CheckSubField() && CheckPatency(LeftUpper.X + Width - 1))
                        LeftUpper.Y -= (LeftUpper.Y - Surface[LeftUpper.X + Width - 1].LeftUpper.Y + Height + 1);
                    dist = DistanceToGround();
                    if (dist < patency)
                        LeftUpper.Y += dist;
                    trajectory.Add(LeftUpper);
                    break;
            }
            dir = direction;
            Visioning(trajectory.ToArray());
            FillingMatr();
            IsMoving = false;
            step++;
        }
        /// <summary>
        /// Функция возврата точки по прямой между двумя точками траектории
        /// </summary>
        /// <param name="x">Абсцисса точки</param>
        /// <param name="p1">Точка 1 для задания прямой</param>
        /// <param name="p2">Точка 2 для задания прямой</param>
        /// <returns></returns>
        private Point GetPointOfLine(int x, Point p1, Point p2)
        {
            if (p2.X - p1.X == 0)
                return new Point(x, p1.Y - 1);
            return new Point(x, (p2.Y - p1.Y) * x / (p2.X - p1.X) + p2.Y - (p2.Y - p1.Y) * p2.X / (p2.X - p1.X));
        }
        /// <summary>
        /// Осуществляет равноускоренное движение червяка с заданной проекцией начальной скорости на оси x и y
        /// </summary>
        /// <param name="speedX">Скорость по координате x</param>
        /// <param name="speedY">Скорость по координате y</param>
        public void Fly(int speedX = 0, int speedY = 0)
        {
            ClearMatr();
            int timeInSpace = 0;
            int curWidth = maxWidth;
            List<Point> trajectory = new List<Point>();
            // Отдельный случай, если герой делает сальто
            if (state != 3)
            {
                state = 2;
                curWidth = curImage.Width;
            }
                
            while (speedX != 0 || speedY != 0 || !PlayerStand())
            {
                LeftUpper.X += speedX;
                LeftUpper.Y += speedY;
                bool flag = false;
                while (LeftUpper.X - difPic < 0 || LeftUpper.X + curWidth>= Owner.Field.GetLength(1) 
                    || LeftUpper.Y < 0
                        || LeftUpper.Y + curImage.Height >= Owner.Field.GetLength(0) || !CheckSubField())
                {
                    if (trajectory.Count == 0)
                    {
                        if (LeftUpper.X + curWidth >= Owner.Field.GetLength(1))
                            LeftUpper.X--;
                        else
                        LeftUpper = GetPointOfLine(LeftUpper.X - Math.Sign(speedX), LeftUpper,
                        LeftUpper);
                    }
                    else
                        LeftUpper = GetPointOfLine(LeftUpper.X - Math.Sign(speedX), LeftUpper,
                            trajectory[trajectory.Count - 1]);
                    flag = true;
                }
                trajectory.Add(LeftUpper);
                if (PlayerStand())
                    break;
                if (flag)
                    speedX = 0;
                timeInSpace++;
                speedY += PlayWindow.g * timeInSpace;
            }
            Visioning(trajectory.ToArray());
            FillingMatr();
            state = 0;
            Show(LeftUpper);
        }
        /// <summary>
        /// Функция, проверяющая есть ли под хитбоксом игрока блок типа Ground
        /// </summary>
        /// <returns></returns>
        public bool PlayerStand()
        {
            Rectangle frame = this;
            for (int i = this.LeftUpper.X; i < this.LeftUpper.X + Width; i++)
                if (Owner.Field[frame.Bottom + 1, i] != 0)
                    return true;
            return false;
        }
        /// <summary>
        /// Процедура прыжка вперёд
        /// </summary>
        public void FrontJump()
        {
            if (!PlayerStand())
                return;
            switch (dir)
            {
                case 0:
                    Fly(-2 * maxSpeed / 3, -(maxSpeed / 2));
                    break;
                case 1:
                    Fly(2 * maxSpeed / 3, -(maxSpeed / 2));
                    break;
            }
        }
        /// <summary>
        /// Процедура прыжка назад
        /// </summary>
        public void BackJump()
        {
            if (!PlayerStand())
                return;
            state = 3;
            switch (dir)
            {
                case 0:
                    Fly(maxSpeed / 2, -maxSpeed);
                    break;
                case 1:
                    Fly(-(maxSpeed / 2), -maxSpeed);
                    break;
            }
        }
    }
}
