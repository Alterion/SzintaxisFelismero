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

        public Tree bal, jobb, szulo;
        public String kif;
        //public String sz;

        public void bejar()
        {
            int x = 0;
            if(kif.Equals("|") || kif.Equals("&") || kif.Equals(">") || kif[0].Equals("V") || kif[0].Equals("J")) Console.Write("(");
            if (this.kif.Equals("!"))
            {
                Console.Write(this.kif);
                x = 1;
            }
            /*Console.WriteLine("Node:" + kif);
            if(szulo != null)
                Console.WriteLine("Szülője " + szulo.kif);*/
            if (bal != null)
            {
                //Console.WriteLine("bal");
                bal.bejar();
            }
            if(x != 1) Console.Write(kif);
            if (jobb != null)
            {
                //Console.WriteLine("jobb");
                jobb.bejar();
            }
            if (kif.Equals("|") || kif.Equals("&") || kif.Equals(">") || kif[0].Equals("V") || kif[0].Equals("J")) Console.Write(")");
            
        }

        //ekvivalencia eltávolítása
        public void ekvivalencia_eltav()
        {
            if (this.kif.Equals("="))
            {
                Tree t = new Tree(">", this);
                Tree t1 = new Tree(">", this);
                this.kif = "&";
                t.bal = this.bal;
                t.jobb = this.jobb;
                t1.bal = this.jobb;
                t1.jobb = this.bal;
                this.bal = t;
                this.jobb = t1;
            }
            if (this.bal != null) this.bal.ekvivalencia_eltav();
            if (this.jobb != null) this.jobb.ekvivalencia_eltav();             
        }

        //implikacio eltávolítása
        public void implikacio_eltav()
        {
            if(this.kif.Equals(">"))
            {
                Tree t;
                if(this.szulo != null)
                {
                    if(this.szulo.kif.Equals("!"))
                    {
                        this.szulo.kif = "&";
                        this.szulo.jobb = new Tree("!", this.szulo);
                        this.jobb.szulo = this.szulo.jobb;
                        this.szulo.jobb.bal = this.jobb;
                        this.jobb = null;
                        this.bal.szulo = this.szulo;
                        this.szulo.bal = this.bal;
                    }
                    else
                    {
                        this.kif = "|";
                        t = new Tree("!", this);
                        t.bal = this.bal;
                        this.bal.szulo = t;
                        this.bal = t;
                    }
                }
                else
                {
                    this.kif = "|";
                    t = new Tree("!", this);
                    t.bal = this.bal;
                    this.bal.szulo = t;
                    this.bal = t;
                }

            }
            if (this.bal != null) this.bal.implikacio_eltav();
            if (this.jobb != null) this.jobb.implikacio_eltav();
        }

        public void negacio_bevitel()
        {
            if(this.szulo == null && this.kif.Equals("!") && this.bal.kif.Equals("!"))
            {
                this.kif = this.bal.bal.kif;
                this.bal = this.bal.bal.bal;
                this.jobb = this.bal.bal.jobb;
            }

            if(this.kif.Equals("!"))
            {
                if(this.bal.kif.Equals("&"))
                {
                    this.kif = "|";
                    this.bal.kif = "!";
                    this.jobb = new Tree("!", this);
                    this.jobb.bal = this.bal.jobb;
                    this.bal.jobb = null;
                }
            }

            if(this.kif.Equals("!"))
            {
                if(this.bal.kif.Equals("|"))
                {
                    this.kif = "&";
                    this.bal.kif = "!";
                    this.jobb = new Tree("!", this);
                    this.jobb.bal = this.bal.jobb;
                    this.bal.jobb = null;
                }
            }

            if (this.bal != null)
            {
                if (this.bal.kif.Equals("!"))
                {
                    if (this.bal.bal.kif.Equals("!"))
                    {
                        this.bal = this.bal.bal.bal;
                        this.bal.szulo = this;
                    }
                }
            }
            if(this.jobb != null)
            {
                if(this.jobb.kif.Equals("!"))
                {
                    if(this.jobb.bal.kif.Equals("!"))
                    {
                        this.jobb = this.jobb.bal.bal;
                        this.jobb.szulo = this;
                    }
                }
            }
            if (this.bal != null) this.bal.negacio_bevitel();
            if (this.jobb != null) this.jobb.negacio_bevitel();
        }

        public void disztribut_konjhoz()
        {
            if (this.kif.Equals("|") && this.jobb.kif.Equals("&"))
            {
                this.kif = "&";
                Tree t = new Tree("|", this);
                t.bal = this.bal.masol(1);
                t.jobb = this.jobb.bal.masol(1);
                this.jobb.kif = "|";
                this.jobb.bal = this.bal.masol(1);
                this.bal = t;
            }

            if (this.kif.Equals("|") && this.bal.kif.Equals("&"))
            {
                this.kif = "&";
                Tree t = new Tree("|", this);
                t.bal = this.bal.jobb.masol(1);
                t.jobb = this.jobb.masol(1);
                this.bal.kif = "|";
                this.bal.jobb = this.jobb.masol(1);
                this.jobb = t;
            }

            if (this.bal != null) this.bal.disztribut_konjhoz();
            if (this.jobb != null) this.jobb.disztribut_konjhoz();
        }

        public void disztribut_diszjhoz()
        {
            if (this.kif.Equals("&") && this.jobb.kif.Equals("|"))
            {
                this.kif = "|";
                Tree t = new Tree("&", this);
                t.bal = this.bal.masol(1);
                t.jobb = this.jobb.bal.masol(1);
                this.jobb.kif = "&";
                this.jobb.bal = this.bal.masol(1);
                this.bal = t;
            }

            if (this.kif.Equals("&") && this.bal.kif.Equals("|"))
            {
                this.kif = "|";
                Tree t = new Tree("&", this);
                t.bal = this.bal.jobb.masol(1);
                t.jobb = this.jobb.masol(1);
                this.bal.kif = "&";
                this.bal.jobb = this.jobb.masol(1);
                this.jobb = t;
            }

            if (this.bal != null) this.bal.disztribut_konjhoz();
            if (this.jobb != null) this.jobb.disztribut_konjhoz();
        }

        public void KNF()
        {
            this.ekvivalencia_eltav();
            this.implikacio_eltav();
            this.negacio_bevitel();
            this.disztribut_konjhoz();
        }

        public void DNF()
        {
            this.ekvivalencia_eltav();
            this.implikacio_eltav();
            this.negacio_bevitel();
            this.disztribut_diszjhoz();
        }

        public Tree egyszerusit()
        {
            if(this.kif.Equals("&") || this.kif.Equals("|"))
            {
                if(this.bal.equals(this.jobb))
                {
                    return this.bal;
                }
            }
            if(this.kif.Equals("|"))
            {
                if(this.bal.kif.Equals("!") && this.bal.bal.equals(this.jobb))
                {
                    return this.jobb;
                }
                if(this.jobb.kif.Equals("!") && this.jobb.bal.equals(this.bal))
                {
                    return this.bal;
                }
            }
            if(this.kif.Equals("&"))
            {
                if (this.bal.kif.Equals("!") && this.bal.bal.equals(this.jobb))
                {
                    return null;
                }
                if (this.jobb.kif.Equals("!") && this.jobb.bal.equals(this.bal))
                {
                    return null;
                }
            }
            if (this.bal != null) this.bal = this.bal.egyszerusit();
            if (this.jobb != null) this.jobb = this.jobb.egyszerusit();
            return this;

        }

        public bool equals(Tree t)
        {
            int x = 0;
            if (this.kif.Equals(t.kif))
                x = 1;
            if(x == 1 && this.bal != null && t.bal != null)
            {
                if (this.bal.equals(t.bal))
                    x = 1;
                else x = 0;
            }
            if(x == 1 && this.jobb != null && t.jobb != null)
            {
                if (this.jobb.equals(t.jobb))
                    x = 1;
                else x = 0;
            }
            if (x == 1)
                return true;
            else return false;
        }
        public Tree masol(int i)
        {
            Tree t;
            if(i == 0)
                t = new Tree("",null);
            else t = new Tree("",this);
            t.kif = this.kif;
            if (bal != null)
                t.bal = this.bal.masol(1);
            if (jobb != null)
                t.jobb = this.jobb.masol(1);
            return t;
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
            //this.sz = "";
            if(s.Length != 0)
                s = zarojelLeszed(s);
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
                            kif = s.Substring(0, 1);
                            bal = new Tree(s.Substring(1, s.Length - 1), this);
                            bal.szulo = this;
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
