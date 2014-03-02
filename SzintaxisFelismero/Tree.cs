using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzintaxisFelismero
{
    class Tree
    {
        enum jelek { EKVIVALENCIA, IMPLIKACIO, KONJUNKCIO, DISZJUNKCIO, NEGACIO, EXISTENCIALIS, UNIVERZALIS, KEZDO }

        Tree bal, jobb, szulo;
        String kif;

        public void bejar()
        {
            Console.WriteLine("Node:" + kif);
            if (bal != null)
            {
                Console.WriteLine("bal");
                bal.bejar();
            }
            if (jobb != null)
            {
                Console.WriteLine("jobb");
                jobb.bejar();
            }
        }

        private String zarojelLeszed(String s)
        {
            int zaro = 0;
            int i, h = 0;
            while (h == 0)
            {
                h = 1;
                if (s[0].Equals('('))
                {
                    for (i = 1; i < s.Length - 1; i++)
                    {
                        if (s[i].Equals('('))
                            zaro++;
                        else if (s[i].Equals(')'))
                            zaro--;
                        if (zaro < 0)
                            return s;
                    }
                    if (zaro == 0 && s[s.Length - 1].Equals(')'))
                    {
                        s = s.Substring(1, s.Length - 2);
                    }
                }
                if (s[0].Equals('('))
                    h = 0;
            }
            return s;
        }

        public Tree(String s, Tree parent)
        {
            //Console.WriteLine(s);
            s = zarojelLeszed(s);
            Console.WriteLine(s);
            if (s.Length == 1)
            {
                kif = s;
                bal = null;
                jobb = null;
                szulo = parent;
            }
            else
            {
                String osszekoto = "formula";
                int i = 0, zaro = 0, op = 0;
                jelek jel = jelek.KEZDO;
                while (i < s.Length)
                {
                    switch (s[i])
                    {
                        case '(': zaro++; break;
                        case ')': zaro--; break;
                        case '=': if (zaro == 0 && jel > jelek.EKVIVALENCIA) { jel = jelek.EKVIVALENCIA; op = i; osszekoto = "="; } break;
                        case '>': if (zaro == 0 && jel > jelek.IMPLIKACIO) { jel = jelek.IMPLIKACIO; op = i; osszekoto = ">"; } break;
                        case '&': if (zaro == 0 && jel > jelek.KONJUNKCIO) { jel = jelek.KONJUNKCIO; op = i; osszekoto = "&"; } break;
                        case '|': if (zaro == 0 && jel > jelek.DISZJUNKCIO) { jel = jelek.DISZJUNKCIO; op = i; osszekoto = "|"; } break;
                        case '!': if (zaro == 0 && jel > jelek.NEGACIO) { jel = jelek.NEGACIO; op = i; osszekoto = "!"; } break;
                        case 'J': if (zaro == 0 && jel > jelek.EXISTENCIALIS) { jel = jelek.EXISTENCIALIS; op = i; osszekoto = "J"; } break;
                        case 'V': if (zaro == 0 && jel > jelek.UNIVERZALIS) { jel = jelek.UNIVERZALIS; op = i; osszekoto = "V"; } break;
                        default: break;
                    }
                    i++;
                }
                if (jel != jelek.NEGACIO && jel != jelek.EXISTENCIALIS && jel != jelek.UNIVERZALIS && op != 0)
                {
                    kif = osszekoto;
                    bal = new Tree(s.Substring(0, op), this);
                    jobb = new Tree(s.Substring(op + 1, s.Length - op - 1), this);
                }
                else
                {
                    if (osszekoto.Equals("formula"))
                    {
                        kif = s;
                        bal = null;
                        jobb = null;
                        szulo = parent;
                    }
                    else
                    {
                        if (jel == jelek.NEGACIO)
                        {
                            kif = "!";
                            bal = new Tree(s.Substring(1, s.Length - 1), this);
                            jobb = null;
                        }
                        else
                        {
                            kif = s.Substring(0, 2);
                            bal = new Tree(s.Substring(2, s.Length - 2), this);
                            jobb = null;
                        }
                    }
                }

            }
        }
    }
}
