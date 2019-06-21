﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Common.Stemmer
{
    public class ArabicStemmer : Stemmer, IStemmer
    {
        private bool B_is_defined;
        private bool B_is_verb;
        private bool B_is_noun;

        private readonly Among[] a_0;
        private readonly Among[] a_1;
        private readonly Among[] a_2;
        private readonly Among[] a_3;
        private readonly Among[] a_4;
        private readonly Among[] a_5;
        private readonly Among[] a_6;
        private readonly Among[] a_7;
        private readonly Among[] a_8;
        private readonly Among[] a_9;
        private readonly Among[] a_10;
        private readonly Among[] a_11;
        private readonly Among[] a_12;
        private readonly Among[] a_13;
        private readonly Among[] a_14;
        private readonly Among[] a_15;
        private readonly Among[] a_16;
        private readonly Among[] a_17;
        private readonly Among[] a_18;
        private readonly Among[] a_19;
        private readonly Among[] a_20;
        private readonly Among[] a_21;

        /// <summary>
        ///   Initializes a new instance of the <see cref="Arabic"/> class.
        /// </summary>
        /// 
        public ArabicStemmer()
        {
            a_0 = new[]
            {
                new Among("\u0640", -1, 1),
                new Among("\u064B", -1, 1),
                new Among("\u064C", -1, 1),
                new Among("\u064D", -1, 1),
                new Among("\u064E", -1, 1),
                new Among("\u064F", -1, 1),
                new Among("\u0650", -1, 1),
                new Among("\u0651", -1, 1),
                new Among("\u0652", -1, 1),
                new Among("\u0660", -1, 2),
                new Among("\u0661", -1, 3),
                new Among("\u0662", -1, 4),
                new Among("\u0663", -1, 5),
                new Among("\u0664", -1, 6),
                new Among("\u0665", -1, 7),
                new Among("\u0666", -1, 8),
                new Among("\u0667", -1, 9),
                new Among("\u0668", -1, 10),
                new Among("\u0669", -1, 11),
                new Among("\uFE80", -1, 12),
                new Among("\uFE81", -1, 16),
                new Among("\uFE82", -1, 16),
                new Among("\uFE83", -1, 13),
                new Among("\uFE84", -1, 13),
                new Among("\uFE85", -1, 17),
                new Among("\uFE86", -1, 17),
                new Among("\uFE87", -1, 14),
                new Among("\uFE88", -1, 14),
                new Among("\uFE89", -1, 15),
                new Among("\uFE8A", -1, 15),
                new Among("\uFE8B", -1, 15),
                new Among("\uFE8C", -1, 15),
                new Among("\uFE8D", -1, 18),
                new Among("\uFE8E", -1, 18),
                new Among("\uFE8F", -1, 19),
                new Among("\uFE90", -1, 19),
                new Among("\uFE91", -1, 19),
                new Among("\uFE92", -1, 19),
                new Among("\uFE93", -1, 20),
                new Among("\uFE94", -1, 20),
                new Among("\uFE95", -1, 21),
                new Among("\uFE96", -1, 21),
                new Among("\uFE97", -1, 21),
                new Among("\uFE98", -1, 21),
                new Among("\uFE99", -1, 22),
                new Among("\uFE9A", -1, 22),
                new Among("\uFE9B", -1, 22),
                new Among("\uFE9C", -1, 22),
                new Among("\uFE9D", -1, 23),
                new Among("\uFE9E", -1, 23),
                new Among("\uFE9F", -1, 23),
                new Among("\uFEA0", -1, 23),
                new Among("\uFEA1", -1, 24),
                new Among("\uFEA2", -1, 24),
                new Among("\uFEA3", -1, 24),
                new Among("\uFEA4", -1, 24),
                new Among("\uFEA5", -1, 25),
                new Among("\uFEA6", -1, 25),
                new Among("\uFEA7", -1, 25),
                new Among("\uFEA8", -1, 25),
                new Among("\uFEA9", -1, 26),
                new Among("\uFEAA", -1, 26),
                new Among("\uFEAB", -1, 27),
                new Among("\uFEAC", -1, 27),
                new Among("\uFEAD", -1, 28),
                new Among("\uFEAE", -1, 28),
                new Among("\uFEAF", -1, 29),
                new Among("\uFEB0", -1, 29),
                new Among("\uFEB1", -1, 30),
                new Among("\uFEB2", -1, 30),
                new Among("\uFEB3", -1, 30),
                new Among("\uFEB4", -1, 30),
                new Among("\uFEB5", -1, 31),
                new Among("\uFEB6", -1, 31),
                new Among("\uFEB7", -1, 31),
                new Among("\uFEB8", -1, 31),
                new Among("\uFEB9", -1, 32),
                new Among("\uFEBA", -1, 32),
                new Among("\uFEBB", -1, 32),
                new Among("\uFEBC", -1, 32),
                new Among("\uFEBD", -1, 33),
                new Among("\uFEBE", -1, 33),
                new Among("\uFEBF", -1, 33),
                new Among("\uFEC0", -1, 33),
                new Among("\uFEC1", -1, 34),
                new Among("\uFEC2", -1, 34),
                new Among("\uFEC3", -1, 34),
                new Among("\uFEC4", -1, 34),
                new Among("\uFEC5", -1, 35),
                new Among("\uFEC6", -1, 35),
                new Among("\uFEC7", -1, 35),
                new Among("\uFEC8", -1, 35),
                new Among("\uFEC9", -1, 36),
                new Among("\uFECA", -1, 36),
                new Among("\uFECB", -1, 36),
                new Among("\uFECC", -1, 36),
                new Among("\uFECD", -1, 37),
                new Among("\uFECE", -1, 37),
                new Among("\uFECF", -1, 37),
                new Among("\uFED0", -1, 37),
                new Among("\uFED1", -1, 38),
                new Among("\uFED2", -1, 38),
                new Among("\uFED3", -1, 38),
                new Among("\uFED4", -1, 38),
                new Among("\uFED5", -1, 39),
                new Among("\uFED6", -1, 39),
                new Among("\uFED7", -1, 39),
                new Among("\uFED8", -1, 39),
                new Among("\uFED9", -1, 40),
                new Among("\uFEDA", -1, 40),
                new Among("\uFEDB", -1, 40),
                new Among("\uFEDC", -1, 40),
                new Among("\uFEDD", -1, 41),
                new Among("\uFEDE", -1, 41),
                new Among("\uFEDF", -1, 41),
                new Among("\uFEE0", -1, 41),
                new Among("\uFEE1", -1, 42),
                new Among("\uFEE2", -1, 42),
                new Among("\uFEE3", -1, 42),
                new Among("\uFEE4", -1, 42),
                new Among("\uFEE5", -1, 43),
                new Among("\uFEE6", -1, 43),
                new Among("\uFEE7", -1, 43),
                new Among("\uFEE8", -1, 43),
                new Among("\uFEE9", -1, 44),
                new Among("\uFEEA", -1, 44),
                new Among("\uFEEB", -1, 44),
                new Among("\uFEEC", -1, 44),
                new Among("\uFEED", -1, 45),
                new Among("\uFEEE", -1, 45),
                new Among("\uFEEF", -1, 46),
                new Among("\uFEF0", -1, 46),
                new Among("\uFEF1", -1, 47),
                new Among("\uFEF2", -1, 47),
                new Among("\uFEF3", -1, 47),
                new Among("\uFEF4", -1, 47),
                new Among("\uFEF5", -1, 51),
                new Among("\uFEF6", -1, 51),
                new Among("\uFEF7", -1, 49),
                new Among("\uFEF8", -1, 49),
                new Among("\uFEF9", -1, 50),
                new Among("\uFEFA", -1, 50),
                new Among("\uFEFB", -1, 48),
                new Among("\uFEFC", -1, 48)
            };

            a_1 = new[]
            {
                new Among("\u0622", -1, 1),
                new Among("\u0623", -1, 1),
                new Among("\u0624", -1, 1),
                new Among("\u0625", -1, 1),
                new Among("\u0626", -1, 1)
            };

            a_2 = new[]
            {
                new Among("\u0622", -1, 1),
                new Among("\u0623", -1, 1),
                new Among("\u0624", -1, 2),
                new Among("\u0625", -1, 1),
                new Among("\u0626", -1, 3)
            };

            a_3 = new[]
            {
                new Among("\u0627\u0644", -1, 2),
                new Among("\u0628\u0627\u0644", -1, 1),
                new Among("\u0643\u0627\u0644", -1, 1),
                new Among("\u0644\u0644", -1, 2)
            };

            a_4 = new[]
            {
                new Among("\u0623\u0622", -1, 2),
                new Among("\u0623\u0623", -1, 1),
                new Among("\u0623\u0624", -1, 1),
                new Among("\u0623\u0625", -1, 4),
                new Among("\u0623\u0627", -1, 3)
            };

            a_5 = new[]
            {
                new Among("\u0641", -1, 1),
                new Among("\u0648", -1, 1)
            };

            a_6 = new[]
            {
                new Among("\u0627\u0644", -1, 2),
                new Among("\u0628\u0627\u0644", -1, 1),
                new Among("\u0643\u0627\u0644", -1, 1),
                new Among("\u0644\u0644", -1, 2)
            };

            a_7 = new[]
            {
                new Among("\u0628", -1, 1),
                new Among("\u0628\u0628", 0, 2),
                new Among("\u0643\u0643", -1, 3)
            };

            a_8 = new[]
            {
                new Among("\u0633\u0623", -1, 4),
                new Among("\u0633\u062A", -1, 2),
                new Among("\u0633\u0646", -1, 3),
                new Among("\u0633\u064A", -1, 1)
            };

            a_9 = new[]
            {
                new Among("\u062A\u0633\u062A", -1, 1),
                new Among("\u0646\u0633\u062A", -1, 1),
                new Among("\u064A\u0633\u062A", -1, 1)
            };

            a_10 = new[]
            {
                new Among("\u0643\u0645\u0627", -1, 3),
                new Among("\u0647\u0645\u0627", -1, 3),
                new Among("\u0646\u0627", -1, 2),
                new Among("\u0647\u0627", -1, 2),
                new Among("\u0643", -1, 1),
                new Among("\u0643\u0645", -1, 2),
                new Among("\u0647\u0645", -1, 2),
                new Among("\u0647\u0646", -1, 2),
                new Among("\u0647", -1, 1),
                new Among("\u064A", -1, 1)
            };

            a_11 = new[]
            {
                new Among("\u0646", -1, 1)
            };

            a_12 = new[]
            {
                new Among("\u0627", -1, 1),
                new Among("\u0648", -1, 1),
                new Among("\u064A", -1, 1)
            };

            a_13 = new[]
            {
                new Among("\u0627\u062A", -1, 1)
            };

            a_14 = new[]
            {
                new Among("\u062A", -1, 1)
            };

            a_15 = new[]
            {
                new Among("\u0629", -1, 1)
            };

            a_16 = new[]
            {
                new Among("\u064A", -1, 1)
            };

            a_17 = new[]
            {
                new Among("\u0643\u0645\u0627", -1, 3),
                new Among("\u0647\u0645\u0627", -1, 3),
                new Among("\u0646\u0627", -1, 2),
                new Among("\u0647\u0627", -1, 2),
                new Among("\u0643", -1, 1),
                new Among("\u0643\u0645", -1, 2),
                new Among("\u0647\u0645", -1, 2),
                new Among("\u0643\u0646", -1, 2),
                new Among("\u0647\u0646", -1, 2),
                new Among("\u0647", -1, 1),
                new Among("\u0643\u0645\u0648", -1, 3),
                new Among("\u0646\u064A", -1, 2)
            };

            a_18 = new[]
            {
                new Among("\u0627", -1, 1),
                new Among("\u062A\u0627", 0, 2),
                new Among("\u062A\u0645\u0627", 0, 4),
                new Among("\u0646\u0627", 0, 2),
                new Among("\u062A", -1, 1),
                new Among("\u0646", -1, 1),
                new Among("\u0627\u0646", 5, 3),
                new Among("\u062A\u0646", 5, 2),
                new Among("\u0648\u0646", 5, 3),
                new Among("\u064A\u0646", 5, 3),
                new Among("\u064A", -1, 1)
            };

            a_19 = new[]
            {
                new Among("\u0648\u0627", -1, 1),
                new Among("\u062A\u0645", -1, 1)
            };

            a_20 = new[]
            {
                new Among("\u0648", -1, 1),
                new Among("\u062A\u0645\u0648", 0, 2)
            };

            a_21 = new[]
            {
                new Among("\u0649", -1, 1)
            };

        }



        private bool r_Normalize_pre()
        {
            int among_var;
            // (, line 246
            // do, line 247
            {
                int c1 = cursor;
                // repeat, line 247
                while (true)
                {
                    int c2 = cursor;
                    // (, line 247
                    // or, line 311
                    {
                        int c3 = cursor;
                        // (, line 248
                        // [, line 249
                        bra = cursor;
                        // substring, line 249
                        among_var = find_among(a_0);
                        if (among_var == 0)
                        {
                            goto lab3;
                        }
                        // ], line 249
                        ket = cursor;
                        switch (among_var)
                        {
                            case 1:
                                // (, line 250
                                // delete, line 250
                                slice_del();
                                break;
                            case 2:
                                // (, line 254
                                // <-, line 254
                                slice_from("0");
                                break;
                            case 3:
                                // (, line 255
                                // <-, line 255
                                slice_from("1");
                                break;
                            case 4:
                                // (, line 256
                                // <-, line 256
                                slice_from("2");
                                break;
                            case 5:
                                // (, line 257
                                // <-, line 257
                                slice_from("3");
                                break;
                            case 6:
                                // (, line 258
                                // <-, line 258
                                slice_from("4");
                                break;
                            case 7:
                                // (, line 259
                                // <-, line 259
                                slice_from("5");
                                break;
                            case 8:
                                // (, line 260
                                // <-, line 260
                                slice_from("6");
                                break;
                            case 9:
                                // (, line 261
                                // <-, line 261
                                slice_from("7");
                                break;
                            case 10:
                                // (, line 262
                                // <-, line 262
                                slice_from("8");
                                break;
                            case 11:
                                // (, line 263
                                // <-, line 263
                                slice_from("9");
                                break;
                            case 12:
                                // (, line 266
                                // <-, line 266
                                slice_from("\u0621");
                                break;
                            case 13:
                                // (, line 267
                                // <-, line 267
                                slice_from("\u0623");
                                break;
                            case 14:
                                // (, line 268
                                // <-, line 268
                                slice_from("\u0625");
                                break;
                            case 15:
                                // (, line 269
                                // <-, line 269
                                slice_from("\u0626");
                                break;
                            case 16:
                                // (, line 270
                                // <-, line 270
                                slice_from("\u0622");
                                break;
                            case 17:
                                // (, line 271
                                // <-, line 271
                                slice_from("\u0624");
                                break;
                            case 18:
                                // (, line 272
                                // <-, line 272
                                slice_from("\u0627");
                                break;
                            case 19:
                                // (, line 273
                                // <-, line 273
                                slice_from("\u0628");
                                break;
                            case 20:
                                // (, line 274
                                // <-, line 274
                                slice_from("\u0629");
                                break;
                            case 21:
                                // (, line 275
                                // <-, line 275
                                slice_from("\u062A");
                                break;
                            case 22:
                                // (, line 276
                                // <-, line 276
                                slice_from("\u062B");
                                break;
                            case 23:
                                // (, line 277
                                // <-, line 277
                                slice_from("\u062C");
                                break;
                            case 24:
                                // (, line 278
                                // <-, line 278
                                slice_from("\u062D");
                                break;
                            case 25:
                                // (, line 279
                                // <-, line 279
                                slice_from("\u062E");
                                break;
                            case 26:
                                // (, line 280
                                // <-, line 280
                                slice_from("\u062F");
                                break;
                            case 27:
                                // (, line 281
                                // <-, line 281
                                slice_from("\u0630");
                                break;
                            case 28:
                                // (, line 282
                                // <-, line 282
                                slice_from("\u0631");
                                break;
                            case 29:
                                // (, line 283
                                // <-, line 283
                                slice_from("\u0632");
                                break;
                            case 30:
                                // (, line 284
                                // <-, line 284
                                slice_from("\u0633");
                                break;
                            case 31:
                                // (, line 285
                                // <-, line 285
                                slice_from("\u0634");
                                break;
                            case 32:
                                // (, line 286
                                // <-, line 286
                                slice_from("\u0635");
                                break;
                            case 33:
                                // (, line 287
                                // <-, line 287
                                slice_from("\u0636");
                                break;
                            case 34:
                                // (, line 288
                                // <-, line 288
                                slice_from("\u0637");
                                break;
                            case 35:
                                // (, line 289
                                // <-, line 289
                                slice_from("\u0638");
                                break;
                            case 36:
                                // (, line 290
                                // <-, line 290
                                slice_from("\u0639");
                                break;
                            case 37:
                                // (, line 291
                                // <-, line 291
                                slice_from("\u063A");
                                break;
                            case 38:
                                // (, line 292
                                // <-, line 292
                                slice_from("\u0641");
                                break;
                            case 39:
                                // (, line 293
                                // <-, line 293
                                slice_from("\u0642");
                                break;
                            case 40:
                                // (, line 294
                                // <-, line 294
                                slice_from("\u0643");
                                break;
                            case 41:
                                // (, line 295
                                // <-, line 295
                                slice_from("\u0644");
                                break;
                            case 42:
                                // (, line 296
                                // <-, line 296
                                slice_from("\u0645");
                                break;
                            case 43:
                                // (, line 297
                                // <-, line 297
                                slice_from("\u0646");
                                break;
                            case 44:
                                // (, line 298
                                // <-, line 298
                                slice_from("\u0647");
                                break;
                            case 45:
                                // (, line 299
                                // <-, line 299
                                slice_from("\u0648");
                                break;
                            case 46:
                                // (, line 300
                                // <-, line 300
                                slice_from("\u0649");
                                break;
                            case 47:
                                // (, line 301
                                // <-, line 301
                                slice_from("\u064A");
                                break;
                            case 48:
                                // (, line 304
                                // <-, line 304
                                slice_from("\u0644\u0627");
                                break;
                            case 49:
                                // (, line 305
                                // <-, line 305
                                slice_from("\u0644\u0623");
                                break;
                            case 50:
                                // (, line 306
                                // <-, line 306
                                slice_from("\u0644\u0625");
                                break;
                            case 51:
                                // (, line 307
                                // <-, line 307
                                slice_from("\u0644\u0622");
                                break;
                        }
                        goto lab2;
                    lab3:;
                        cursor = c3;
                        // next, line 312
                        if (cursor >= limit)
                        {
                            goto lab1;
                        }
                        cursor++;
                    }
                lab2:;
                    continue;
                lab1:;
                    cursor = c2;
                    break;
                }
                cursor = c1;
            }
            return true;
        }

        private bool r_Normalize_post()
        {
            int among_var;
            // (, line 316
            // do, line 318
            {
                int c1 = cursor;
                // (, line 318
                // backwards, line 320
                limit_backward = cursor;
                cursor = limit;
                // (, line 320
                // [, line 321
                ket = cursor;
                // substring, line 321
                if (find_among_b(a_1) == 0)
                {
                    goto lab0;
                }
                // ], line 321
                bra = cursor;
                // (, line 322
                // <-, line 322
                slice_from("\u0621");
                cursor = limit_backward;
            lab0:;
                cursor = c1;
            }
            // do, line 329
            {
                int c2 = cursor;
                // repeat, line 329
                while (true)
                {
                    int c3 = cursor;
                    // (, line 329
                    // or, line 338
                    {
                        int c4 = cursor;
                        // (, line 330
                        // [, line 332
                        bra = cursor;
                        // substring, line 332
                        among_var = find_among(a_2);
                        if (among_var == 0)
                        {
                            goto lab4;
                        }
                        // ], line 332
                        ket = cursor;
                        switch (among_var)
                        {
                            case 1:
                                // (, line 333
                                // <-, line 333
                                slice_from("\u0627");
                                break;
                            case 2:
                                // (, line 334
                                // <-, line 334
                                slice_from("\u0648");
                                break;
                            case 3:
                                // (, line 335
                                // <-, line 335
                                slice_from("\u064A");
                                break;
                        }
                        goto lab3;
                    lab4:;
                        cursor = c4;
                        // next, line 339
                        if (cursor >= limit)
                        {
                            goto lab2;
                        }
                        cursor++;
                    }
                lab3:;
                    continue;
                lab2:;
                    cursor = c3;
                    break;
                }
                cursor = c2;
            }
            return true;
        }

        private bool r_Checks1()
        {
            int among_var;
            // (, line 344
            // [, line 345
            bra = cursor;
            // substring, line 345
            among_var = find_among(a_3);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 345
            ket = cursor;
            switch (among_var)
            {
                case 1:
                    // (, line 346
                    if (!(current.Length > 4))
                    {
                        return false;
                    }
                    // set is_noun, line 346
                    B_is_noun = true;
                    // unset is_verb, line 346
                    B_is_verb = false;
                    // set is_defined, line 346
                    B_is_defined = true;
                    break;
                case 2:
                    // (, line 347
                    if (!(current.Length > 3))
                    {
                        return false;
                    }
                    // set is_noun, line 347
                    B_is_noun = true;
                    // unset is_verb, line 347
                    B_is_verb = false;
                    // set is_defined, line 347
                    B_is_defined = true;
                    break;
            }
            return true;
        }

        private bool r_Prefix_Step1()
        {
            int among_var;
            // (, line 353
            // [, line 354
            bra = cursor;
            // substring, line 354
            among_var = find_among(a_4);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 354
            ket = cursor;
            switch (among_var)
            {
                case 1:
                    // (, line 355
                    if (!(current.Length > 3))
                    {
                        return false;
                    }
                    // <-, line 355
                    slice_from("\u0623");
                    break;
                case 2:
                    // (, line 356
                    if (!(current.Length > 3))
                    {
                        return false;
                    }
                    // <-, line 356
                    slice_from("\u0622");
                    break;
                case 3:
                    // (, line 358
                    if (!(current.Length > 3))
                    {
                        return false;
                    }
                    // <-, line 358
                    slice_from("\u0627");
                    break;
                case 4:
                    // (, line 359
                    if (!(current.Length > 3))
                    {
                        return false;
                    }
                    // <-, line 359
                    slice_from("\u0625");
                    break;
            }
            return true;
        }

        private bool r_Prefix_Step2()
        {
            // (, line 364
            // not, line 365
            {
                int c1 = cursor;
                // literal, line 365
                if (!(eq_s("\u0641\u0627")))
                {
                    goto lab0;
                }
                return false;
            lab0:;
                cursor = c1;
            }
            // not, line 366
            {
                int c2 = cursor;
                // literal, line 366
                if (!(eq_s("\u0648\u0627")))
                {
                    goto lab1;
                }
                return false;
            lab1:;
                cursor = c2;
            }
            // [, line 367
            bra = cursor;
            // substring, line 367
            if (find_among(a_5) == 0)
            {
                return false;
            }
            // ], line 367
            ket = cursor;
            // (, line 368
            if (!(current.Length > 3))
            {
                return false;
            }
            // delete, line 368
            slice_del();
            return true;
        }

        private bool r_Prefix_Step3a_Noun()
        {
            int among_var;
            // (, line 373
            // [, line 374
            bra = cursor;
            // substring, line 374
            among_var = find_among(a_6);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 374
            ket = cursor;
            switch (among_var)
            {
                case 1:
                    // (, line 375
                    if (!(current.Length > 5))
                    {
                        return false;
                    }
                    // delete, line 375
                    slice_del();
                    break;
                case 2:
                    // (, line 376
                    if (!(current.Length > 4))
                    {
                        return false;
                    }
                    // delete, line 376
                    slice_del();
                    break;
            }
            return true;
        }

        private bool r_Prefix_Step3b_Noun()
        {
            int among_var;
            // (, line 380
            // not, line 381
            {
                int c1 = cursor;
                // literal, line 381
                if (!(eq_s("\u0628\u0627")))
                {
                    goto lab0;
                }
                return false;
            lab0:;
                cursor = c1;
            }
            // [, line 382
            bra = cursor;
            // substring, line 382
            among_var = find_among(a_7);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 382
            ket = cursor;
            switch (among_var)
            {
                case 1:
                    // (, line 383
                    if (!(current.Length > 3))
                    {
                        return false;
                    }
                    // delete, line 383
                    slice_del();
                    break;
                case 2:
                    // (, line 385
                    if (!(current.Length > 3))
                    {
                        return false;
                    }
                    // <-, line 385
                    slice_from("\u0628");
                    break;
                case 3:
                    // (, line 386
                    if (!(current.Length > 3))
                    {
                        return false;
                    }
                    // <-, line 386
                    slice_from("\u0643");
                    break;
            }
            return true;
        }

        private bool r_Prefix_Step3_Verb()
        {
            int among_var;
            // (, line 391
            // [, line 392
            bra = cursor;
            // substring, line 392
            among_var = find_among(a_8);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 392
            ket = cursor;
            switch (among_var)
            {
                case 1:
                    // (, line 394
                    if (!(current.Length > 4))
                    {
                        return false;
                    }
                    // <-, line 394
                    slice_from("\u064A");
                    break;
                case 2:
                    // (, line 395
                    if (!(current.Length > 4))
                    {
                        return false;
                    }
                    // <-, line 395
                    slice_from("\u062A");
                    break;
                case 3:
                    // (, line 396
                    if (!(current.Length > 4))
                    {
                        return false;
                    }
                    // <-, line 396
                    slice_from("\u0646");
                    break;
                case 4:
                    // (, line 397
                    if (!(current.Length > 4))
                    {
                        return false;
                    }
                    // <-, line 397
                    slice_from("\u0623");
                    break;
            }
            return true;
        }

        private bool r_Prefix_Step4_Verb()
        {
            // (, line 401
            // [, line 402
            bra = cursor;
            // substring, line 402
            if (find_among(a_9) == 0)
            {
                return false;
            }
            // ], line 402
            ket = cursor;
            // (, line 403
            if (!(current.Length > 4))
            {
                return false;
            }
            // set is_verb, line 403
            B_is_verb = true;
            // unset is_noun, line 403
            B_is_noun = false;
            // <-, line 403
            slice_from("\u0627\u0633\u062A");
            return true;
        }

        private bool r_Suffix_Noun_Step1a()
        {
            int among_var;
            // (, line 410
            // [, line 411
            ket = cursor;
            // substring, line 411
            among_var = find_among_b(a_10);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 411
            bra = cursor;
            switch (among_var)
            {
                case 1:
                    // (, line 412
                    if (!(current.Length >= 4))
                    {
                        return false;
                    }
                    // delete, line 412
                    slice_del();
                    break;
                case 2:
                    // (, line 413
                    if (!(current.Length >= 5))
                    {
                        return false;
                    }
                    // delete, line 413
                    slice_del();
                    break;
                case 3:
                    // (, line 414
                    if (!(current.Length >= 6))
                    {
                        return false;
                    }
                    // delete, line 414
                    slice_del();
                    break;
            }
            return true;
        }

        private bool r_Suffix_Noun_Step1b()
        {
            // (, line 417
            // [, line 418
            ket = cursor;
            // substring, line 418
            if (find_among_b(a_11) == 0)
            {
                return false;
            }
            // ], line 418
            bra = cursor;
            // (, line 419
            if (!(current.Length > 5))
            {
                return false;
            }
            // delete, line 419
            slice_del();
            return true;
        }

        private bool r_Suffix_Noun_Step2a()
        {
            // (, line 423
            // [, line 424
            ket = cursor;
            // substring, line 424
            if (find_among_b(a_12) == 0)
            {
                return false;
            }
            // ], line 424
            bra = cursor;
            // (, line 425
            if (!(current.Length > 4))
            {
                return false;
            }
            // delete, line 425
            slice_del();
            return true;
        }

        private bool r_Suffix_Noun_Step2b()
        {
            // (, line 429
            // [, line 430
            ket = cursor;
            // substring, line 430
            if (find_among_b(a_13) == 0)
            {
                return false;
            }
            // ], line 430
            bra = cursor;
            // (, line 431
            if (!(current.Length >= 5))
            {
                return false;
            }
            // delete, line 431
            slice_del();
            return true;
        }

        private bool r_Suffix_Noun_Step2c1()
        {
            // (, line 435
            // [, line 436
            ket = cursor;
            // substring, line 436
            if (find_among_b(a_14) == 0)
            {
                return false;
            }
            // ], line 436
            bra = cursor;
            // (, line 437
            if (!(current.Length >= 4))
            {
                return false;
            }
            // delete, line 437
            slice_del();
            return true;
        }

        private bool r_Suffix_Noun_Step2c2()
        {
            // (, line 440
            // [, line 441
            ket = cursor;
            // substring, line 441
            if (find_among_b(a_15) == 0)
            {
                return false;
            }
            // ], line 441
            bra = cursor;
            // (, line 442
            if (!(current.Length >= 4))
            {
                return false;
            }
            // delete, line 442
            slice_del();
            return true;
        }

        private bool r_Suffix_Noun_Step3()
        {
            // (, line 445
            // [, line 446
            ket = cursor;
            // substring, line 446
            if (find_among_b(a_16) == 0)
            {
                return false;
            }
            // ], line 446
            bra = cursor;
            // (, line 447
            if (!(current.Length >= 3))
            {
                return false;
            }
            // delete, line 447
            slice_del();
            return true;
        }

        private bool r_Suffix_Verb_Step1()
        {
            int among_var;
            // (, line 451
            // [, line 452
            ket = cursor;
            // substring, line 452
            among_var = find_among_b(a_17);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 452
            bra = cursor;
            switch (among_var)
            {
                case 1:
                    // (, line 453
                    if (!(current.Length >= 4))
                    {
                        return false;
                    }
                    // delete, line 453
                    slice_del();
                    break;
                case 2:
                    // (, line 454
                    if (!(current.Length >= 5))
                    {
                        return false;
                    }
                    // delete, line 454
                    slice_del();
                    break;
                case 3:
                    // (, line 455
                    if (!(current.Length >= 6))
                    {
                        return false;
                    }
                    // delete, line 455
                    slice_del();
                    break;
            }
            return true;
        }

        private bool r_Suffix_Verb_Step2a()
        {
            int among_var;
            // (, line 458
            // [, line 459
            ket = cursor;
            // substring, line 459
            among_var = find_among_b(a_18);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 459
            bra = cursor;
            switch (among_var)
            {
                case 1:
                    // (, line 460
                    if (!(current.Length >= 4))
                    {
                        return false;
                    }
                    // delete, line 460
                    slice_del();
                    break;
                case 2:
                    // (, line 462
                    if (!(current.Length >= 5))
                    {
                        return false;
                    }
                    // delete, line 462
                    slice_del();
                    break;
                case 3:
                    // (, line 463
                    if (!(current.Length > 5))
                    {
                        return false;
                    }
                    // delete, line 463
                    slice_del();
                    break;
                case 4:
                    // (, line 464
                    if (!(current.Length >= 6))
                    {
                        return false;
                    }
                    // delete, line 464
                    slice_del();
                    break;
            }
            return true;
        }

        private bool r_Suffix_Verb_Step2b()
        {
            // (, line 468
            // [, line 469
            ket = cursor;
            // substring, line 469
            if (find_among_b(a_19) == 0)
            {
                return false;
            }
            // ], line 469
            bra = cursor;
            // (, line 470
            if (!(current.Length >= 5))
            {
                return false;
            }
            // delete, line 470
            slice_del();
            return true;
        }

        private bool r_Suffix_Verb_Step2c()
        {
            int among_var;
            // (, line 475
            // [, line 476
            ket = cursor;
            // substring, line 476
            among_var = find_among_b(a_20);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 476
            bra = cursor;
            switch (among_var)
            {
                case 1:
                    // (, line 477
                    if (!(current.Length >= 4))
                    {
                        return false;
                    }
                    // delete, line 477
                    slice_del();
                    break;
                case 2:
                    // (, line 478
                    if (!(current.Length >= 6))
                    {
                        return false;
                    }
                    // delete, line 478
                    slice_del();
                    break;
            }
            return true;
        }

        private bool r_Suffix_All_alef_maqsura()
        {
            // (, line 482
            // [, line 483
            ket = cursor;
            // substring, line 483
            if (find_among_b(a_21) == 0)
            {
                return false;
            }
            // ], line 483
            bra = cursor;
            // (, line 484
            // <-, line 484
            slice_from("\u064A");
            return true;
        }

        protected override bool stem()
        {
            // (, line 491
            // set is_noun, line 493
            B_is_noun = true;
            // set is_verb, line 494
            B_is_verb = true;
            // unset is_defined, line 495
            B_is_defined = false;
            // do, line 498
            {
                int c1 = cursor;
                // call Checks1, line 498
                if (!r_Checks1())
                    goto lab0;
                lab0:;
                cursor = c1;
            }
            // do, line 501
            // call Normalize_pre, line 501
            if (!r_Normalize_pre())
                goto lab1;
            lab1:;
            // backwards, line 504
            limit_backward = cursor;
            cursor = limit;
            // (, line 504
            // do, line 506
            {
                int c3 = limit - cursor;
                // (, line 506
                // or, line 520
                {
                    int c4 = limit - cursor;
                    // (, line 508
                    // Boolean test is_verb, line 509
                    if (!(B_is_verb))
                    {
                        goto lab4;
                    }
                    // (, line 510
                    // or, line 515
                    {
                        int c5 = limit - cursor;
                        // (, line 511
                        // (, line 512
                        // atleast, line 512
                        {
                            int c6 = 1;
                            // atleast, line 512
                            while (true)
                            {
                                int c7 = limit - cursor;
                                // call Suffix_Verb_Step1, line 512
                                if (!r_Suffix_Verb_Step1())
                                    goto lab7;
                                c6--;
                                continue;
                            lab7:;
                                cursor = limit - c7;
                                break;
                            }
                            if (c6 > 0)
                            {
                                goto lab6;
                            }
                        }
                        // (, line 513
                        // or, line 513
                        {
                            int c8 = limit - cursor;
                            // call Suffix_Verb_Step2a, line 513
                            if (!r_Suffix_Verb_Step2a())
                                goto lab9;
                            goto lab8;
                        lab9:;
                            cursor = limit - c8;
                            // call Suffix_Verb_Step2c, line 513
                            if (!r_Suffix_Verb_Step2c())
                                goto lab10;
                            goto lab8;
                        lab10:;
                            cursor = limit - c8;
                            // next, line 513
                            if (cursor <= limit_backward)
                            {
                                goto lab6;
                            }
                            cursor--;
                        }
                    lab8:;
                        goto lab5;
                    lab6:;
                        cursor = limit - c5;
                        // call Suffix_Verb_Step2b, line 515
                        if (!r_Suffix_Verb_Step2b())
                            goto lab11;
                        goto lab5;
                    lab11:;
                        cursor = limit - c5;
                        // call Suffix_Verb_Step2a, line 516
                        if (!r_Suffix_Verb_Step2a())
                            goto lab4;
                    }
                lab5:;
                    goto lab3;
                lab4:;
                    cursor = limit - c4;
                    // (, line 520
                    // Boolean test is_noun, line 521
                    if (!(B_is_noun))
                    {
                        goto lab12;
                    }
                    // (, line 522
                    // try, line 524
                    {
                        int c9 = limit - cursor;
                        // (, line 524
                        // or, line 526
                        {
                            int c10 = limit - cursor;
                            // call Suffix_Noun_Step2c2, line 525
                            if (!r_Suffix_Noun_Step2c2())
                                goto lab15;
                            goto lab14;
                        lab15:;
                            cursor = limit - c10;
                            // (, line 526
                            // not, line 526
                            // Boolean test is_defined, line 526
                            if (!(B_is_defined))
                            {
                                goto lab17;
                            }
                            goto lab16;
                        lab17:;
                            // call Suffix_Noun_Step1a, line 526
                            if (!r_Suffix_Noun_Step1a())
                                goto lab16;
                            // (, line 526
                            // or, line 528
                            {
                                int c12 = limit - cursor;
                                // call Suffix_Noun_Step2a, line 527
                                if (!r_Suffix_Noun_Step2a())
                                    goto lab19;
                                goto lab18;
                            lab19:;
                                cursor = limit - c12;
                                // call Suffix_Noun_Step2b, line 528
                                if (!r_Suffix_Noun_Step2b())
                                    goto lab20;
                                goto lab18;
                            lab20:;
                                cursor = limit - c12;
                                // call Suffix_Noun_Step2c1, line 529
                                if (!r_Suffix_Noun_Step2c1())
                                    goto lab21;
                                goto lab18;
                            lab21:;
                                cursor = limit - c12;
                                // next, line 530
                                if (cursor <= limit_backward)
                                {
                                    goto lab16;
                                }
                                cursor--;
                            }
                        lab18:;
                            goto lab14;
                        lab16:;
                            cursor = limit - c10;
                            // (, line 531
                            // call Suffix_Noun_Step1b, line 531
                            if (!r_Suffix_Noun_Step1b())
                                goto lab22;
                            // (, line 531
                            // or, line 533
                            {
                                int c13 = limit - cursor;
                                // call Suffix_Noun_Step2a, line 532
                                if (!r_Suffix_Noun_Step2a())
                                    goto lab24;
                                goto lab23;
                            lab24:;
                                cursor = limit - c13;
                                // call Suffix_Noun_Step2b, line 533
                                if (!r_Suffix_Noun_Step2b())
                                    goto lab25;
                                goto lab23;
                            lab25:;
                                cursor = limit - c13;
                                // call Suffix_Noun_Step2c1, line 534
                                if (!r_Suffix_Noun_Step2c1())
                                    goto lab22;
                            }
                        lab23:;
                            goto lab14;
                        lab22:;
                            cursor = limit - c10;
                            // (, line 535
                            // not, line 535
                            // Boolean test is_defined, line 535
                            if (!(B_is_defined))
                            {
                                goto lab27;
                            }
                            goto lab26;
                        lab27:;
                            // call Suffix_Noun_Step2a, line 535
                            if (!r_Suffix_Noun_Step2a())
                                goto lab26;
                            goto lab14;
                        lab26:;
                            cursor = limit - c10;
                            // (, line 536
                            // call Suffix_Noun_Step2b, line 536
                            if (!r_Suffix_Noun_Step2b())
                            {
                                cursor = limit - c9;
                                goto lab13;
                            }
                        }
                    lab14:;
                    lab13:;
                    }
                    // call Suffix_Noun_Step3, line 538
                    if (!r_Suffix_Noun_Step3())
                        goto lab12;
                    goto lab3;
                lab12:;
                    cursor = limit - c4;
                    // call Suffix_All_alef_maqsura, line 544
                    if (!r_Suffix_All_alef_maqsura())
                        goto lab2;
                }
            lab3:;
            lab2:;
                cursor = limit - c3;
            }
            cursor = limit_backward;
            // do, line 549
            {
                int c15 = cursor;
                // (, line 549
                // try, line 550
                {
                    int c16 = cursor;
                    // call Prefix_Step1, line 550
                    if (!r_Prefix_Step1())
                    {
                        cursor = c16;
                        goto lab29;
                    }
                lab29:;
                }
                // try, line 551
                {
                    int c17 = cursor;
                    // call Prefix_Step2, line 551
                    if (!r_Prefix_Step2())
                    {
                        cursor = c17;
                        goto lab30;
                    }
                lab30:;
                }
                // (, line 552
                // or, line 553
                {
                    int c18 = cursor;
                    // call Prefix_Step3a_Noun, line 552
                    if (!r_Prefix_Step3a_Noun())
                        goto lab32;
                    goto lab31;
                lab32:;
                    cursor = c18;
                    // (, line 553
                    // Boolean test is_noun, line 553
                    if (!(B_is_noun))
                    {
                        goto lab33;
                    }
                    // call Prefix_Step3b_Noun, line 553
                    if (!r_Prefix_Step3b_Noun())
                        goto lab33;
                    goto lab31;
                lab33:;
                    cursor = c18;
                    // (, line 554
                    // Boolean test is_verb, line 554
                    if (!(B_is_verb))
                    {
                        goto lab28;
                    }
                    // try, line 554
                    {
                        int c19 = cursor;
                        // call Prefix_Step3_Verb, line 554
                        if (!r_Prefix_Step3_Verb())
                        {
                            cursor = c19;
                            goto lab34;
                        }
                    lab34:;
                    }
                    // call Prefix_Step4_Verb, line 554
                    if (!r_Prefix_Step4_Verb())
                        goto lab28;
                }
            lab31:;
            lab28:;
                cursor = c15;
            }
            // do, line 559
            // call Normalize_post, line 559
            if (!r_Normalize_post())
                goto lab35;
            lab35:;
            return true;
        }
    }
}
