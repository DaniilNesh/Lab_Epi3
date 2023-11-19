using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Epi_Lab3
{
    public partial class Form1 : Form
    {
        double expenses;
        double price;
        (double, double) limits;
        List<double> diagValues;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e){}
        private void label1_Click(object sender, EventArgs e){}
        /// <summary>
        /// Рассчёт значений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //double[] texts = new double[15];
            //TextBox[] boxes = new TextBox[15] {text1, text2, text3, text4, text5, text6, text7, text8, text9, text10, 
            //    text11, text12, text13, text14, text15 };

            //for(int i = 0; i < 15; i++)
            //{
            //    if(!double.TryParse(boxes[i].Text, out texts[i]))
            //    {
            //        MessageBox.Show(
            //            "Текст не обработан!",
            //            "Ошибка",
            //            MessageBoxButtons.OK
            //            );
            //        return;
            //    }
            //}
            // из методички
            //double[] texts = new double[] { 110, 497.08, 0.11, 55, 0.2, 2.85, 11930, 1, 15000, 50, 35, 20, 20, 18, 11 };
            // из варианта
            double[] texts = new double[] { 150, 600, 1.02, 54, 0.5, 2.8, 12000, 1, 22000, 70, 55, 24, 14, 29, 15 };
            Values values = new Values(texts);

            expenses = Calculation.expenses(values, out diagValues);
            price = Calculation.setting_price(expenses, values.Rent);
            limits = Calculation.get_limits(expenses, values);

            string str = $"Общее время разработки ПО: {texts[0]}\n Оплата труда: {texts[1]}\n " +
                $"Коэффициент, учитывающий дополнительную заработную плату: {texts[2]}\n Процент накладных расходов: {texts[3]}\n" +
                $"Потребляемая мощность: {texts[4]}\n Цена 1кВт-ч электроэнергии: {texts[5]}\n Заработная плата персонала в месяц: {texts[6]}\n" +
                $"Кол-во обслуживаемых единиц: {texts[7]}\n Балансовая стоимость компьютера: {texts[8]}\n Время кодирования: {texts[9]}\n" +
                $"Время отладки: {texts[10]}\n Рентабельность: {texts[11]}\n Тиражирование: {texts[12]}\n НДС: {texts[13]}\n Дополнительная прибыль: {texts[14]}\n" +
                $"Затраты: {expenses}\n Цена: {price}\n Нижний предел цены: {limits.Item1.ToString()}\n Договорная цена: {limits.Item2.ToString()}\n ";

            using (StreamWriter writer = new StreamWriter("Result.txt"))
            {
                writer.WriteLine(str);
            }

            textBox16.Text = expenses.ToString();
            textBox17.Text = price.ToString();
            textBox18.Text = limits.Item1.ToString();
            textBox19.Text = limits.Item2.ToString();
        }
        /// <summary>
        /// Запрет ввода буквенно-символьных значений в поля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            var value = e.KeyChar;
            if (value <= 47 || value >= 58)
            {
                if (value == 8 || value == 44)
                    return;
                e.Handled = true;
            }
        }
        /// <summary>
        /// Рисование диаграмм
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if(diagValues == null)
            {
                MessageBox.Show(
                        "Нет значений для отрисовки!",
                        "Ошибка",
                        MessageBoxButtons.OK
                        );
                return;
            }

            chart1.Series.Clear();
            chart1.Titles.Clear();
            chart1.Titles.Add("Результаты решения");
            var ex = chart1.Series.Add("Затраты");
            ex.Points.Add(expenses);
            var pr = chart1.Series.Add("Цена");
            pr.Points.Add(price);
            var limit1 = chart1.Series.Add("Нижний предел");
            limit1.Points.Add(limits.Item1);
            var limit2 = chart1.Series.Add("Договорная цена");
            limit2.Points.Add(limits.Item2);
            chart1.Series["Затраты"].IsValueShownAsLabel = true;
            chart1.Series["Цена"].IsValueShownAsLabel = true;
            chart1.Series["Нижний предел"].IsValueShownAsLabel = true;
            chart1.Series["Договорная цена"].IsValueShownAsLabel = true;

            chart2.Series["Series1"].Points.Clear();
            chart2.Titles.Clear();
            chart2.Titles.Add("Результаты решения");
            for(int i = 0; i<diagValues.Count; i++)
            {
                var index = get_Name(i);
                chart2.Series["Series1"].Points.AddXY(index, diagValues[i]);
            }
            
        }
        /// <summary>
        /// Возрващает имя для значения диаграммы
        /// </summary>
        /// <param name="index">номер элемента</param>
        /// <returns>имя значения</returns>
        private string get_Name(int index)
        {
            if (index == 0)
                return "Зр";
            else if (index == 1)
                return "Зд";
            else if (index == 2)
                return "Зс";
            else if (index == 3)
                return "Зн";
            else if (index == 4)
                return "Рэкспл";
            else
                return string.Empty;
        }
    }
}
