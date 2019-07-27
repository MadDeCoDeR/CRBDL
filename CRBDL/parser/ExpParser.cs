/*
Classic RBDoom 3 BFG Edition Launcher

Copyright(C) 2019 George Kalmpokis

Permission is hereby granted, free of charge, to any person obtaining a copy of this software
and associated documentation files(the "Software"), to deal in the Software without
restriction, including without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following conditions :

The above copyright notice and this permission notice shall be included in all copies or substantial
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
namespace CDL.parser
{
    class ExpParser
    {
        public static int getD2MExp(string name)
        {
            switch (name)
            {
                case "TNT: Evilution":
                    return 1;
                case "The Plutonia Experiment":
                    return 2;
                case "Master Levels":
                    return 3;
                case "No Rest For the Living":
                    return 4;
                default:
                    return 0;
            }
        }

        public static string setD2MExp(int value)
        {
            switch (value)
            {
                case 1:
                    return "TNT: Evilution";
                case 2:
                    return "The Plutonia Experiment";
                case 3:
                    return "Master Levels";
                case 4:
                    return "No Rest For the Living";
                default:
                    return "Hell on Earth";
            }
        }

        public static int getD2Exp(string name)
        {
            switch (name)
            {
                case "Hell on Earth":
                    return 1;
                case "TNT: Evilution":
                    return 3;
                case "The Plutonia Experiment":
                    return 4;
                case "Master Levels":
                    return 5;
                case "No Rest For the Living":
                    return 2;
                default:
                    return 0;
            }
        }

        public string setD2Exp(int value)
        {
            switch (value)
            {
                case 1:
                    return "Hell on Earth";
                case 3:
                    return "TNT: Evilution";
                case 4:
                    return "The Plutonia Experiment";
                case 5:
                    return "Master Levels";
                case 2:
                    return "No Rest For the Living";
                default:
                    return "(none)";
            }
        }
    }
}
