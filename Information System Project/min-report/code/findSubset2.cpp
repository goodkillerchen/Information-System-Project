void findSubsetOfkOptim(int n,int k, vector<int> subsetK){
    int count=0;//number of 1s
    for(int i = (1<<k)-1 ; i < (1<<n); i++){
        for(int j = 0; j < n; j++){
            //the binary number representation 
            //of subset has an 1 on the jth position
            if(i & (1<<j)!=0){
                count++;
            }
        }
        if(count==k)
            susetK.empalce_back(i);
        count=0;
    }

}