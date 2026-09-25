using System;

[Serializable]
public class InformationPageData
{
    public string name;
    public string description;
}

[Serializable]
public class InformationPageDataWrapper // butuh ini karna JsonUtility ga bisa read raw json array
{
    public InformationPageData[] pages;
}
