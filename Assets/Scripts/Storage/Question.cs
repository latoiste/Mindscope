using System;

[Serializable]
public class Question
{
    public string type;
    public string question;
    public string answer;
    public string clue;
}

[Serializable]
public class QuestionWrapper
{
    public Question[] questions;
}
