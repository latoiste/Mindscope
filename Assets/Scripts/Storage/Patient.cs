using System;

[Serializable]
public class Patient
{
    public string id;
    public string name;
    public string age;
    public string occupation;
    public string reasonForVisiting;
    public string[] possibleDiseases;
    public string controllerPath;
}

[Serializable]
public class PatientWrapper
{
    public Patient[] patients;
}