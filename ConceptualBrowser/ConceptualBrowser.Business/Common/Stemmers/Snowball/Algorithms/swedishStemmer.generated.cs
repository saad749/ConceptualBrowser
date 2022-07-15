// Generated by Snowball 2.2.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;
    
    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated by Snowball 2.2.0 - https://snowballstem.org/
    ///</summary>
    /// 
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "2.2.0")]
    public partial class SwedishStemmer : Stemmer
    {
        private int I_x;
        private int I_p1;

        private const string g_v = "aeiouy\u00E4\u00E5\u00F6";
        private const string g_s_ending = "bcdfghjklmnoprtvy";
        private static readonly Among[] a_0 = new[] 
        {
            new Among("a", -1, 1),
            new Among("arna", 0, 1),
            new Among("erna", 0, 1),
            new Among("heterna", 2, 1),
            new Among("orna", 0, 1),
            new Among("ad", -1, 1),
            new Among("e", -1, 1),
            new Among("ade", 6, 1),
            new Among("ande", 6, 1),
            new Among("arne", 6, 1),
            new Among("are", 6, 1),
            new Among("aste", 6, 1),
            new Among("en", -1, 1),
            new Among("anden", 12, 1),
            new Among("aren", 12, 1),
            new Among("heten", 12, 1),
            new Among("ern", -1, 1),
            new Among("ar", -1, 1),
            new Among("er", -1, 1),
            new Among("heter", 18, 1),
            new Among("or", -1, 1),
            new Among("s", -1, 2),
            new Among("as", 21, 1),
            new Among("arnas", 22, 1),
            new Among("ernas", 22, 1),
            new Among("ornas", 22, 1),
            new Among("es", 21, 1),
            new Among("ades", 26, 1),
            new Among("andes", 26, 1),
            new Among("ens", 21, 1),
            new Among("arens", 29, 1),
            new Among("hetens", 29, 1),
            new Among("erns", 21, 1),
            new Among("at", -1, 1),
            new Among("andet", -1, 1),
            new Among("het", -1, 1),
            new Among("ast", -1, 1)
        };

        private static readonly Among[] a_1 = new[] 
        {
            new Among("dd", -1, -1),
            new Among("gd", -1, -1),
            new Among("nn", -1, -1),
            new Among("dt", -1, -1),
            new Among("gt", -1, -1),
            new Among("kt", -1, -1),
            new Among("tt", -1, -1)
        };

        private static readonly Among[] a_2 = new[] 
        {
            new Among("ig", -1, 1),
            new Among("lig", 0, 1),
            new Among("els", -1, 1),
            new Among("fullt", -1, 3),
            new Among("l\u00F6st", -1, 2)
        };



        private bool r_mark_regions()
        {
            I_p1 = limit;
            {
                int c1 = cursor;
                {
                    int c = cursor + 3;
                    if (c > limit)
                    {
                        return false;
                    }
                    cursor = c;
                }
                I_x = cursor;
                cursor = c1;
            }
            if (out_grouping(g_v, 97, 246, true) < 0)
            {
                return false;
            }

            {

                int ret = in_grouping(g_v, 97, 246, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            I_p1 = cursor;
            if (!(I_p1 < I_x))
            {
                goto lab0;
            }
            I_p1 = I_x;
        lab0: ; 
            return true;
        }

        private bool r_main_suffix()
        {
            int among_var;
            if (cursor < I_p1)
            {
                return false;
            }
            int c2 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            among_var = find_among_b(a_0);
            if (among_var == 0)
            {
                {
                    limit_backward = c2;
                    return false;
                }
            }
            bra = cursor;
            limit_backward = c2;
            switch (among_var) {
                case 1:
                    slice_del();
                    break;
                case 2:
                    if (in_grouping_b(g_s_ending, 98, 121, false) != 0)
                    {
                        return false;
                    }
                    slice_del();
                    break;
            }
            return true;
        }

        private bool r_consonant_pair()
        {
            if (cursor < I_p1)
            {
                return false;
            }
            int c2 = limit_backward;
            limit_backward = I_p1;
            int c3 = limit - cursor;
            if (find_among_b(a_1) == 0)
            {
                {
                    limit_backward = c2;
                    return false;
                }
            }
            cursor = limit - c3;
            ket = cursor;
            if (cursor <= limit_backward)
            {
                {
                    limit_backward = c2;
                    return false;
                }
            }
            cursor--;
            bra = cursor;
            slice_del();
            limit_backward = c2;
            return true;
        }

        private bool r_other_suffix()
        {
            int among_var;
            if (cursor < I_p1)
            {
                return false;
            }
            int c2 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            among_var = find_among_b(a_2);
            if (among_var == 0)
            {
                {
                    limit_backward = c2;
                    return false;
                }
            }
            bra = cursor;
            switch (among_var) {
                case 1:
                    slice_del();
                    break;
                case 2:
                    slice_from("l\u00F6s");
                    break;
                case 3:
                    slice_from("full");
                    break;
            }
            limit_backward = c2;
            return true;
        }

        protected override bool stem()
        {
            {
                int c1 = cursor;
                r_mark_regions();
                cursor = c1;
            }
            limit_backward = cursor;
            cursor = limit;
            {
                int c2 = limit - cursor;
                r_main_suffix();
                cursor = limit - c2;
            }
            {
                int c3 = limit - cursor;
                r_consonant_pair();
                cursor = limit - c3;
            }
            {
                int c4 = limit - cursor;
                r_other_suffix();
                cursor = limit - c4;
            }
            cursor = limit_backward;
            return true;
        }

    }
}
