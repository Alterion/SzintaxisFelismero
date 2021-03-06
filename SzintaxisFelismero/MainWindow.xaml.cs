﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SzintaxisFelismero
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum allapotok { KEZDO, KVANTOR, PREDIKATUM_BETU, KOV_ARG, PREDIKATUM, TERM, ARGUMENTUM }
        

        List<Predikatum> predikatumok = new List<Predikatum>();
        List<Fuggveny> fuggvenyek = new List<Fuggveny>();
        Stack<char> szimbolum = new Stack<char>();
        Stack<int> argumentum = new Stack<int>();

        public MainWindow()
        {
            InitializeComponent();
            Tree.valtozok = new int[26];
            Tree.valtozokNullaz();
        }

        
        
        private bool joFormula(string s)
        {
            predikatumok = new List<Predikatum>();
            fuggvenyek = new List<Fuggveny>();
            szimbolum = new Stack<char>();
            argumentum = new Stack<int>();

            allapotok allapot = allapotok.KEZDO;
            int zaro = 0, z2 = 0;

            for (int i = 0; i < s.Length; i++)
            {
                switch (allapot)
                {
                    case allapotok.KEZDO:
                        if (s[i] == 'V' || s[i] == 'J') { allapot = allapotok.KVANTOR; }
                        else if (s[i] == '!') { allapot = allapotok.KEZDO; }
                        else if (s[i] == '(') { z2++; }
                        else /*if (i + 1 <= s.Length - 1) { */if (s[i] >= 65 && s[i] <= 90 && s[i + 1] == '(') { allapot = allapotok.PREDIKATUM; /*}*/ }
                        else if (s[i] >= 65 && s[i] <= 90) { allapot = allapotok.PREDIKATUM_BETU; }
                        else { eredmenyLabel.Content = "Hibás formula!"; return false; }
                        if (allapot == allapotok.PREDIKATUM || allapot == allapotok.PREDIKATUM_BETU)
                        {
                            foreach (Fuggveny f in fuggvenyek)
                            {
                                if (f.szimb.Equals(s[i]))
                                {
                                    //Console.WriteLine("predikátum is és függvény is!");
                                    eredmenyLabel.Content = "A(z) " + s[i] + " jel egyszer függvény máskor predikátumszimbólumként szerepel!";
                                    return false;
                                }
                            }
                            int x = 0;
                            foreach (Predikatum p in predikatumok)
                            {
                                if (p.szimb.Equals(s[i]))
                                    x = 1;
                            }
                            if (x == 0)
                                predikatumok.Add(new Predikatum(s[i], 0));
                        }
                        if (allapot == allapotok.PREDIKATUM_BETU && s[i] >= 65 && s[i] <= 90)
                        {
                            foreach (Predikatum p in predikatumok)
                            {
                                if (p.szimb.Equals(s[i]))
                                {
                                    if (p.parameterek > 0)
                                    {
                                        //Console.WriteLine("hibás paraméterszám!");
                                        eredmenyLabel.Content = "A(z) " + s[i] + " predikátum hibás paraméterszámmal szerepel!";
                                        return false;
                                    }
                                    p.parameterek = -1;
                                }
                            }
                        }
                        break;

                    case allapotok.KVANTOR:
                        if (s[i] >= 97 && s[i] <= 122 && s[i] != 'c' && s[i] != 'd' && s[i] != 'e') { allapot = allapotok.KEZDO; }
                        else
                        {
                            eredmenyLabel.Content = "A kvantor után csak változó állhat!";
                            return false;
                        }
                        break;

                    case allapotok.PREDIKATUM_BETU:
                        if (s[i] == '|') { allapot = allapotok.KEZDO; }
                        else if (s[i] == '>') { allapot = allapotok.KEZDO; }
                        else if (s[i] == '&') { allapot = allapotok.KEZDO; }
                        else if (s[i] == '=') { allapot = allapotok.KEZDO; }
                        else if (s[i] == ')' & z2 > 0) { z2--; }
                        else return false;
                        break;

                    case allapotok.KOV_ARG:
                        if (s[i] >= 65 && s[i] <= 90 && s[i + 1] == '(') { allapot = allapotok.PREDIKATUM; }
                        else if (s[i] >= 65 && s[i] <= 90) { /*Console.WriteLine("Egy fgv. szimbólumnak minimum egy araméterrel rendelkeznie kell!!");*/ eredmenyLabel.Content = "A(z)" + s[i] + "függvényszimbólumnak nincs paraéter megadva!"; return false; }
                        else if (s[i] >= 97 && s[i] <= 122) { allapot = allapotok.ARGUMENTUM; }
                        else return false;
                        if ((allapot == allapotok.PREDIKATUM || allapot == allapotok.ARGUMENTUM) && s[i] >= 65 && s[i] <= 90)
                        {
                            foreach (Predikatum p in predikatumok)
                            {
                                if (p.szimb.Equals(s[i]))
                                {
                                    //Console.WriteLine("predikátum is és függvény is!");
                                    eredmenyLabel.Content = "A(z) " + s[i] + " jel egyszer függvény máskor predikátumszimbólumként szerepel!";
                                    return false;
                                }
                            }
                            int x = 0;
                            foreach (Fuggveny f in fuggvenyek)
                            {
                                if (f.szimb.Equals(s[i]))
                                    x = 1;
                            }
                            if (x == 0)
                                fuggvenyek.Add(new Fuggveny(s[i], 0));
                        }
                        break;

                    case allapotok.TERM:
                        if (s[i] >= 97 && s[i] <= 122) { allapot = allapotok.ARGUMENTUM; }
                        else if (s[i] >= 65 && s[i] <= 90 && s[i + 1] == '(') { allapot = allapotok.PREDIKATUM; }
                        else return false;
                        if (allapot == allapotok.PREDIKATUM)
                        {
                            foreach (Predikatum p in predikatumok)
                            {
                                if (p.szimb.Equals(s[i]))
                                {
                                    //Console.WriteLine("predikátum is és függvény is!");
                                    eredmenyLabel.Content = "A(z) " + s[i] + " jel egyszer függvény máskor predikátumszimbólumként szerepel!";
                                    return false;
                                }
                            }
                            int x = 0;
                            foreach (Fuggveny f in fuggvenyek)
                            {
                                if (f.szimb.Equals(s[i]))
                                    x = 1;
                            }
                            if (x == 0)
                                fuggvenyek.Add(new Fuggveny(s[i], 0));
                        }
                        break;

                    case allapotok.PREDIKATUM:
                        allapot = allapotok.TERM; zaro++;
                        szimbolum.Push(s[i - 1]);
                        argumentum.Push(1);
                        break;

                    case allapotok.ARGUMENTUM:
                        if (s[i] == ')')
                        {
                            zaro--;
                            int a = 0;
                            foreach (Predikatum p in predikatumok)
                            {
                                if (p.szimb.Equals(szimbolum.Peek()))
                                    a = p.parameterek;
                            }
                            foreach (Fuggveny f in fuggvenyek)
                            {
                                if (f.szimb.Equals(szimbolum.Peek()))
                                    a = f.parameterek;
                            }

                            if (argumentum.Peek() != a && a != 0)
                            {
                                //Console.WriteLine("Hibás paraméterszám:");
                                eredmenyLabel.Content = "Hibás paraméterszám!";
                                return false;
                            }
                            char c = szimbolum.Pop();
                            foreach (Predikatum p in predikatumok)
                            {
                                if (p.szimb.Equals(c))
                                    p.parameterek = argumentum.Pop();
                            }
                            foreach (Fuggveny f in fuggvenyek)
                            {
                                if (f.szimb.Equals(c))
                                    f.parameterek = argumentum.Pop();
                            }
                        }
                        else if (s[i] == ',') { allapot = allapotok.KOV_ARG; argumentum.Push(argumentum.Pop() + 1); }
                        else return false;
                        if (zaro == 0) { allapot = allapotok.PREDIKATUM_BETU; }
                        break;


                    default: return false;
                }
                Console.WriteLine(allapot);
            }

            if (z2 == 0 && allapot == allapotok.PREDIKATUM_BETU)
                return true;
            else return false;

        }


        private void inditButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("---------------------------------------");
            String s = inputBox.Text;
            Tree t = new Tree(s, null);           
            if (joFormula(inputBox.Text))
            {
                knfLabel.Content = "";
                dnfLabel.Content = "";
                t.prenexizalo();
                Tree t2 = t.masol(0);
                /*t.KNF();
                t2.DNF();
                /*t.bejar(knfLabel);
                Console.WriteLine();
                t2.bejar(dnfLabel);
                Console.WriteLine();*/
                t = t.egyszerusit();
                t2 = t2.egyszerusit();
                if (t != null)
                {
                    t = t.uresAgEltavolit();
                    t.bejar(knfLabel);
                    Console.WriteLine();
                }
                else knfLabel.Content = "üres";
                /*if (t2 != null)
                {
                    t2 = t2.uresAgEltavolit();
                    t2.bejar(dnfLabel);
                    Console.WriteLine();
                }
                else dnfLabel.Content = "üres";       */                     
                eredmenyLabel.Content = "Helyes formula!";                
            }
            else eredmenyLabel.Content = "Hibás formula!";
        }
    }
    
}
