using System;

[Serializable]
public class DiagnosisPageData
{
    public string spriteId;
    public string name;
}

[Serializable]
public class DiagnosisPageDataWrapper // butuh ini karna JsonUtility ga bisa read raw json array
{
    public DiagnosisPageData[] pages;
}
