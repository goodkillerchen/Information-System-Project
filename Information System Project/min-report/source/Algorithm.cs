using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography;
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
        private List<int> setNumberForK = new List<int>();
        private List<int> setNumberForJ = new List<int>();
        private Dictionary<int, int> dictionaryForAllSets 
            = new Dictionary<int, int>();
        private Dictionary<int, int> dictionaryForAllSets2 
            = new Dictionary<int, int>();
        private bool[] visit = new bool[10000000];
        private List<int> totalList = new List<int>();
        private Queue<int> queueForSet = new Queue<int>();
        private bool[] judgeNumber = new bool[46];
        private int[,] C = new int[26,26];
        public Algorithm(int n, int k, int j, int s,
            List<int> totalList, bool[] judgeNumber)
        {
            this.n = n;
            this.k = k;
            this.j = j;
            this.s = s;
            this.totalList = totalList;
            this.judgeNumber = judgeNumber;
        }
        public Queue<int> ExecuteAlgorithm1()
        {
            GreedyAlgorithm();
            return queueForSet;
        }
        public Queue<int> ExecuteAlgorithm2() 
        { 
            GreedyAlgorithm2();
            return queueForSet;
        }
        private void GreedyAlgorithm()
        {
            setNumberForK=CombinationForAllNum(n, k);
            int max;
            int now = 0;
            int allNum = TotalNumberForJ(n,j);
            int node;
            int index;
            List<int> vis = new List<int>();
            List<int> result = new List<int>();
            while (allNum > 0)
            {
                //Debug.WriteLine(allNum);
                max = 0;
                node = 0;
                index = 0;
                foreach (var element 
                    in setNumberForK)
                {
                    int numOfUnfound = 0;
                    for (int j1 = (1 << s) - 1; 
                        j1 <= element; j1++)
                    {
                        int cnt = 0;
                        for (int k1 = 0; k1 < n; k1++)
                        {
                            if ((j1 & (1 << k1)) != 0)
                            {
                                cnt++;
                            }
                        }
                        if ((j1 & element) == j1 
                            && cnt == s && 
                                !dictionaryForAllSets.
                                    ContainsKey(j1))
                        {
                            numOfUnfound++;
                            vis.Add(j1);
                        }
                    }
                    if (max < numOfUnfound)
                    {
                        node = index;
                        max = numOfUnfound;
                        result.Clear();
                        foreach(var eachNum in vis)
                        {
                            result.Add(eachNum);
                        }
                    }
                    vis.Clear();
                    index++;
                }
                queueForSet.Enqueue(setNumberForK[node]);
                setNumberForK.RemoveAt(node);
                foreach(var eachNum in result)
                {
                    dictionaryForAllSets[eachNum] = ++now;
                }
                allNum -= max;
                
            }

        }
        private void GreedyAlgorithm2() 
        {
            setNumberForK=CombinationForAllNum(n, k);
            int max;
            int now = 0;
            int allNum = TotalNumberForJ(n, j);
            int node;
            int index;
            List<int> vis = new List<int>();
            List<int> result = new List<int>();
            while (allNum > 0)
            {
                max = 0;
                node = 0;
                index = 0;
                foreach (var element in setNumberForK)
                {
                    int numOfUnfound = 0;
                    for (int j1 = (1 << s) - 1; 
                        j1 <= element; j1++)
                    {
                        int cnt = 0;
                        var answer = 0;
                        for (int k1 = 0; k1 < n; k1++)
                        {
                            if ((j1 & (1 << k1)) != 0)
                            {
                                cnt++;
                            }
                        }
                        if ((j1 & element) == j1 
                            && cnt == s && 
                                !dictionaryForAllSets.
                                    ContainsKey(j1))
                        {
                            int num = j - s;
                            numOfSetContainj1
                                (0,j1, num,ref answer);
                            numOfUnfound += answer;
                            vis.Add(j1);
                        }
                    }
                    foreach(var eachNum in setNumberForJ)
                    {
                        dictionaryForAllSets2.Remove(eachNum);
                    }
                    setNumberForJ.Clear();
                    if (max < numOfUnfound)
                    {
                        max = numOfUnfound;
                        node = index;
                        result.Clear();
                        foreach(var eachNum in vis)
                        {
                            result.Add(eachNum);
                        }
                    }
                    index++;
                    vis.Clear();
                }
                //Debug.WriteLine(max);
                SetDictionary2(result);
                queueForSet.Enqueue(setNumberForK[node]);
                foreach(var eachNum in result)
                {
                    //Debug.Write(eachNum + " ");
                    dictionaryForAllSets[eachNum] = ++now;
                }
                setNumberForK.RemoveAt(node);
                allNum -= max;
            }
        }
        private void numOfSetContainj1(int node, int j1, 
            int num,ref int answer)
        {
            if (num == 0 && 
                !dictionaryForAllSets2.
                    ContainsKey(j1))
            {
                answer += 1;
                setNumberForJ.Add(j1);
                dictionaryForAllSets2[j1] = 
                    dictionaryForAllSets2.Count() + 1;
                return;
            }
            else if (num == 0 && 
                dictionaryForAllSets2.
                    ContainsKey(j1))
                return;
            for(int i1 = node; i1 < n; i1++)
            {
                if(((1<<i1) & j1) == 0)
                {
                    numOfSetContainj1(i1+1, j1 | (1 << i1),
                        num - 1, ref answer);
                }
            }
        }

        private void SetDictionary2(List<int> result)
        {
            foreach(var eachNum in result)
            {
                FindEachElement(0,eachNum,j-s);
            }
        }

        private void FindEachElement(int node,int element,
            int num)
        {
            if (num == 0 && 
                !dictionaryForAllSets2.
                    ContainsKey(element))
            {
                dictionaryForAllSets2[element] 
                    = dictionaryForAllSets2.Count() + 1;
                return;
            }
            else if (num == 0 && 
                dictionaryForAllSets2.
                    ContainsKey(element))
                return;
            for (int i1 = node; i1 < n; i1++)
            {
                if (((1 << i1) & element) == 0)
                {
                    FindEachElement(i1, element | (1 << i1), 
                        num - 1);
                }
            }
        }

        private List<int> CombinationForAllNum(int n,int k)
        {
            List<int> Combination = new List<int>();
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
                    Combination.Add(i);
                    //myBV.Add(new BitVector32(i));
                }
            }
            return Combination;
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
        
        private void Dfs(int start,int setNum,
            int currentNumber,int totalNum,ref int result)
        {

            int now = currentNumber;
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
                    for (int j1 = (1 << s) - 1; 
                        j1 <= setNumberForK[i]; j1++)
                    {
                        int cnt = 0;
                        for (int k1 = 0; k1 < n; k1++)
                        {
                            if ((j1 & (1 << k1)) != 0)
                            {
                                cnt++;
                            }
                        }
                        if ((j1 & setNumberForK[i])==j1 
                            && cnt == s && 
                                !dictionaryForAllSets.
                                    ContainsKey(j1))
                        {
                            now++;
                            dictionaryForAllSets[j1] = now;
                            vis.Add(j1);
                        }
                    }
                    //Debug.WriteLine(" ");
                    Dfs(i + 1, setNum + 1, now, 
                        totalNum, ref result);
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
