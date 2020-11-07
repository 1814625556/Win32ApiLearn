using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace SpeechToText
{
    class Program
    {
        private static SpeechRecognitionEngine engine;
        static void Main(string[] args)
        {
            engine = new SpeechRecognitionEngine();
            engine.SetInputToDefaultAudioDevice();         //<=======默认的语音输入设备，你可以设定为去识别一个WAV文件。
            var gb = new GrammarBuilder();
            //需要判断的文本（相当于语音库）
            gb.Append(new Choices(new string[] { "检查", "姓名", "继续", "选择", "向上", "向下", "向左", "向右", "取消", "确定", "Go", "Ok", "Yes", "No", "Hello" }));
            Grammar g = new Grammar(gb);
            engine.LoadGrammar(g);
            engine.RecognizeAsync(RecognizeMode.Multiple);
            engine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(G_SpeechRecognized);
            Console.ReadLine();
        }

        /// <summary>
        /// 判断语音并转化为需要输出的文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void G_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string result = e.Result.Text;
            Console.WriteLine(result);
            Speak(result);
            //speak(RetSpeck);
        }

        private static void Speak(string str)
        {
            using (var speech = new SpeechSynthesizer())
            {
                speech.Speak($"Are you speaking {str}"); //语音方法调用
            }
        }
    }
}
