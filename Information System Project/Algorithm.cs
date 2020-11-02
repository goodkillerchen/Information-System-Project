using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Information_System_Project
{
    public class Algorithm
    {
        private int n;
        private int k;
        private int j;
        private int s;
        List<int> setNumberForK = new List<int>();
        Dictionary<int,int> dictionaryForAllSets=new Dictionary<int,int>();
        bool[] visit = new bool[10000000];
        List<int> totalList = new List<int>();
        bool[] judgeNumber = new bool[46];
        int[,] C = new int[26,26];
        public Algorithm(int n, int k, int j, int s,List<int> totalList, bool[] judgeNumber)
        {
            this.n = n;
            this.k = k;
            this.j = j;
            this.s = s;
            this.totalList = totalList;
            this.judgeNumber = judgeNumber;
        }
        public int EexcuteAlgorithm()
        {
            for (int i = 0; i < visit.Length; i++)
                visit[i] = false;
            CombinationForAllK(n, k);
            int totalNum=TotalNumberForJ(n, j);
            var result=int.MaxValue;
            Dfs(0,0,0, totalNum,ref result);
            return result;

        }
        private void CombinationForAllK(int n,int k)
        {
            for (int i = (1<<(k))-1; i < (1 << n); i++)
            {
                var cnt = 0;
                for (int j1 = 0; j1 < n; j1++)
                {
                    if ((i & (1 << j1)) != 0)
                    {
                        cnt++;
                    }
                }
                if (cnt == k)
                {
                    setNumberForK.Add(i);
                    //myBV.Add(new BitVector32(i));
                }
            }
        }
        private int TotalNumberForJ(int n,int j)
        { 
            for(int i = 0; i <= n; i++)
            {
                for(int m = 0; m <= j; m++)
                {
                    C[i, m] = 0;
                }
            }
            for(int i = 0; i <= n; i++)
            {
                C[i, 0] = 1;
                if (i == n && j == 0)
                    return C[n, j];
            }
            for (int i = 1; i <= n; i++)
            {
                for (int m = 1; m <= i; m++)
                {
                    C[i, m] = C[i - 1, m - 1] + C[i-1, m];
                    if (i == n && m == j)
                        return C[n, m];
                }
            }
            return C[n, j];
        }
        private void Dfs(int start,int setNum,int currentNumber,int totalNum,ref int result)
        {
            int now = currentNumber;
            Debug.WriteLine(setNum+" "+ currentNumber+" "+totalNum+" "+result);
            
            List<int> vis = new List<int>();
            if (setNum >= result)
                return;
            if (currentNumber == totalNum)
            {
                
                //if (setNum < result)
                result = setNum;
                return;
            }
            for (int i = start; i < setNumberForK.Count; i++)
            {
                if (!visit[i])
                {
                    for (int j1 = (1 << s) - 1; j1 < setNumberForK[i]; j1++)
                    {
                        int cnt = 0;
                        for (int k1 = 0; k1 < n; k1++)
                        {
                            if ((j1 & (1 << k1)) != 0)
                            {
                                cnt++;
                            }
                        }
                        if ((j1 & setNumberForK[i])==j1 && cnt == s && !dictionaryForAllSets.ContainsKey(j1))//
                        {
                            now++;
                            dictionaryForAllSets[j1] = now;
                            vis.Add(j1);
                            //Debug.WriteLine(now + " " + currentNumber+" "+j1+" "+setNumberForK[i]);
                        }
                    }
                    //Debug.WriteLine(" ");
                    Dfs(i + 1, setNum + 1, now, totalNum, ref result);
                    visit[i] = false;
                    now = currentNumber;
                   
                }
                foreach (var eachNum in vis)
                {
                    dictionaryForAllSets.Remove(eachNum);
                }
                vis.Clear();
            }
            return ;
        }
    }
}
