using System;
using System.Collections.Generic;


namespace Epi_Lab3
{
    struct Values
    {
        public double Trazr;    //Общее время разработки ПО
        public double Otr;      //Оплата труда
        public double Coeffd;   //Коэффициент, учитывающий дополнительную заработную плату
        public double procRas;  //Процент накладных расходов
        public double Mp;       //Потребляемая мощность
        public double priceE;   //Цена 1кВт-ч электроэнергии
        public double ZP0;      //Заработная плата персонала в месяц
        public double KolObsh;  //Кол-во обслуживаемых единиц
        public double balprice; //Балансовая стоимость компьютера
        public double Tkod;     //Время кодирования
        public double Totl;     //Время отладки
        public double Rent;     //Рентабельность
        public double Tirazh;   //Тиражирование
        public double NDS;      //НДС
        public double dopPr;    //Дополнительная прибыль

        public Values(double[] array)
        {
            Trazr = array[0];
            Otr = array[1];
            Coeffd = array[2];
            procRas = array[3];
            Mp = array[4];
            priceE = array[5];
            ZP0 = array[6];
            KolObsh = array[7];
            balprice = array[8];
            Tkod = array[9];
            Totl = array[10];
            Rent = array[11];
            Tirazh = array[12];
            NDS = array[13];
            dopPr = array[14];
        }
    }
    class Calculation
    {
        public static double expenses(Values values, out List<double> items)
        {
            items = new List<double>();
            const double NormA = 50;
            var ZPrazr = Math.Round(values.Trazr * values.Otr, 2);
            items.Add(ZPrazr);
            var ZPdop = Math.Round(ZPrazr * values.Coeffd, 2);
            items.Add(ZPdop);
            var ZPsoc = Math.Round((ZPrazr + ZPdop) * 0.356, 2);
            items.Add(ZPsoc);
            var Zras = Math.Round(ZPrazr * values.procRas / 100, 2);
            items.Add(Zras);
            double Fg = 8 * 288;
            var sumE = Math.Round(values.Mp * Fg * values.priceE, 2);
            var RasComp = Math.Round(values.ZP0 * 12 / values.KolObsh, 2);
            double Amort = Math.Round(NormA * values.balprice / 100, 2);
            var RashRem = Math.Round(0.05 * values.balprice, 2);
            var PriceOneHour = Math.Round((sumE + RasComp + Amort + RashRem) / Fg, 2);
            var Pexpl = Math.Round((values.Tkod + values.Totl) * 8 * PriceOneHour, 2);
            items.Add(Pexpl);
            return Math.Round(ZPrazr + ZPdop + ZPsoc + Zras + Pexpl, 2);
        }
        public static double setting_price(double expenses, double Rent)
        {
            return expenses * (1 + Rent / 100);
        }
        public static (double, double) get_limits(double expenses, Values values)
        {
            var low_limit = expenses * (1 + values.Rent / 100) / values.Tirazh * (1 + values.NDS / 100);
            var dogovor_price = low_limit * (1 + values.dopPr / 100);
            return (Math.Round(low_limit, 2), Math.Round(dogovor_price, 2));
        }
    }
}
