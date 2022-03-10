using HW7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HW7.Controllers
{
    public class MyController : Controller
    {
        static Random myRand = new Random();
        List<MyModel> myListModel = new List<MyModel>(16);
        // GET: My

        public async Task<ActionResult> Index()
        {
            await Task.Run(() => shuffle());
            return View(myListModel);
        }
        public void shuffle()
        {
            short i;
            int[] arr= new int[16];
            for (i = 0; i < 15; i++)
                arr[i] = i + 1;
            arr[15] = -1;
            for (i = 14; i > 0; i--) 
            {
                int rand = myRand.Next(i);
                int temp = arr[i];
                arr[i] = arr[rand];
                arr[rand] = temp;
            }
            for( i = 0; i < 16; i++)
            {
                MyModel tempModel = new MyModel();
                if(i != 15)
                {
                    tempModel.text = arr[i].ToString();
                    tempModel.hexRGB = myRand.Next(100, 256).ToString("x") + myRand.Next(100, 256).ToString("x") + myRand.Next(100, 256).ToString("x");
                }
                /*else
                {
                    tempModel.text = "-1";
                    tempModel.hexRGB = "FFFFF";
                }*/
                myListModel.Add(tempModel);
            }
        }
/*        public ActionResult Index(MyModel myM, string myButton)
        {
            if(myButton=="+")
                myM.result = myM.num1+myM.num2;
            if (myButton == "-")
                myM.result = myM.num1 - myM.num2;
            return View(myM);
            
        }*/
        [HttpGet]
        public async Task<JsonResult> shuffleAction()
        {
            await Task.Run(() => shuffle());
            return Json(myListModel,JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public async Task<JsonResult> currentStepAction(int indexPushed, string colorPushed, int indexEmpty, string colorEmpty)
        {
            MyModel resultModel;
            resultModel = await Task.Run(() => currentStep(indexPushed, colorPushed, indexEmpty, colorEmpty));
            return Json(resultModel);
        }

        public MyModel currentStep(int indexPushed, string colorPushed,int indexEmpty, string colorEmpty)
        {
            int i_1 = indexPushed % 4;
            int j_1 = indexPushed / 4;

            int i_2 = indexEmpty % 4;
            int j_2 = indexEmpty / 4;

            MyModel resultModel =  new MyModel();
            if (Math.Abs(i_1 - i_2) + Math.Abs(j_1 - j_2) != 1)
                resultModel.text = "false";
            else
            {
                resultModel.text = "true";

                string strR_Pushed = colorPushed.Substring(1, 2);
                int R_Pushed = int.Parse(strR_Pushed,System.Globalization.NumberStyles.HexNumber);
                string strB_Pushed = colorPushed.Substring(3, 2);
                int B_Pushed = int.Parse(strB_Pushed, System.Globalization.NumberStyles.HexNumber);
                string strG_Pushed = colorPushed.Substring(5, 2);
                int G_Pushed = int.Parse(strG_Pushed, System.Globalization.NumberStyles.HexNumber);

                string strR_Empty = colorEmpty.Substring(1, 2);
                int R_Empty = int.Parse(strR_Empty, System.Globalization.NumberStyles.HexNumber);
                string strB_Empty = colorEmpty.Substring(3, 2);
                int B_Empty = int.Parse(strB_Empty, System.Globalization.NumberStyles.HexNumber);
                string strG_Empty = colorEmpty.Substring(5, 2);
                int G_Empty = int.Parse(strG_Empty, System.Globalization.NumberStyles.HexNumber);

                int R = (R_Pushed + R_Empty) / 2;
                int G = (G_Pushed + G_Empty) / 2;
                int B = (B_Pushed + B_Empty) / 2;

                resultModel.hexRGB = "#" + R.ToString("x") + G.ToString("x") + B.ToString("x");
            }
            return resultModel;
        }
        [HttpPost]
        public async Task<bool> isEndAction(string[] arrStr)
        {
            bool result;
            result = await Task.Run(() => isEnd(arrStr));
            return result;
        }
        public bool isEnd(string[] arrStr)
        {
            for(int i = 0; i < 2; i++)
            {
                if(arrStr[i] != (i + 1).ToString())
                {
                    return false;
                }
            }
            return true;
        }
    }
}