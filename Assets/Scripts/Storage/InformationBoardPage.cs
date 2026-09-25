using System;

[Serializable]
public class PageData
{
    public string name;
    public string description;
}

[Serializable]
public class PageDataWrapper // butuh ini karna JsonUtility ga bisa read raw json array
{
    public PageData[] pages;
}
