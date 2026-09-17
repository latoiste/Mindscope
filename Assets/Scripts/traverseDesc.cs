using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class traverseDesc : MonoBehaviour
{
    private string[] titles;
    private string[] descsss;
    public GameObject leftArrow, leftArrowButton;
    public GameObject rightArrow, rightArrowButton;

    public traverseDesc otherArrow;
    public bool isLeftArrow;
    private int currPage;

    private int firstPage, lastPage;

    public TextMeshProUGUI title;
    public TextMeshProUGUI desc;
    void Start()
    {
        initialize();

        currPage = 0;
        firstPage = 0;
        lastPage = titles.Length - 1;

        if (currPage - 1 < firstPage)
        {
            leftArrow.SetActive(false);
            leftArrow.SetActive(false);
        }

        if (currPage + 1 > lastPage)
        {
            rightArrow.SetActive(false);
            rightArrow.SetActive(false);
        }

        title.text = titles[currPage];
        desc.text = descsss[currPage];
    }

    private void initialize()
    {
        titles = new string[3];
        titles[0] = "THE HOLLOW";
        titles[1] = "THE SIGNAL";
        titles[2] = "THE REMNANT";

        descsss = new string[3];
        descsss[0] = "Also known as Major Depression Disorder\n\n";
        descsss[0] += "A serious mood disorder characterized by persistent feelings of sadness, despair and loss of interest in pleasureable activities.";
        descsss[0] += "Unlike temporary emotional responses to life challenges, MDD is a long-term condition that can significantly interfere with daily functioning";
        descsss[0] += "\n\nMight include symptoms of : \n• Constant feeling of sadness or feeling of emptiness";
        descsss[0] += "\n• Anhedonia or loss of interest or loss of joy in\n  previously pleasureable activities";
        descsss[0] += "\n• Difficulty concentrating, making decisions, or\n  remembering things";
        descsss[0] += "\n• Trouble sleeping, excessive sleeping, irregular sleep\n  cycles";
        descsss[0] += "\n• Excessive feelings of unworthiness or guilt";
        descsss[0] += "\n• Thoughts of death, suicide, or even attempts to end\n  their life";

        descsss[1] = "Also known as Generalized Anxiety Disorder\n\n";
        descsss[1] += "Mental health conditions characterized by excessive anxiety that interferes with daily life. This anxiety it not only related to ordinary stress or fear"; ;
        descsss[1] += ", but is an excessive response to certain situations that can affect work, social relationships, and daily activities. This disorder can appear without a clear trigger and last for a long period of time";
        descsss[1] += "\n\nMight include symptoms of :";
        descsss[1] += "\n• Expressing excessive anxiety about various aspects of\n  life, even if there is no obvious reason to feel anxious";
        descsss[1] += "\n• Feeling awake or physically tense, such as stiff\n  muscles, and often feeling anxious or worried";
        descsss[1] += "\n• Trouble sleeping or waking up in the middle of sleep\n  because of ongoing anxiety";
        descsss[1] += "\n• Difficulty concentrating or finding it difficult to focus\n  on daily tasks due to constant anxiety";

        descsss[2] = "Also known as Post-Traumatic Stress Disorder\n\n";
        descsss[2] += "Mental disorders that occur fater experiencing or witnessing a traumatic event that is very frightening or life-threatening. PTSD can cause feelings of anxiety, being trapped in disturbing memories";
        descsss[2] += ", and avoidance of situations that remind the individual of the trauma. This condition can last a long time, even years after the traumatic event.";
        descsss[2] += "\n\nMight include symptoms of : ";
        descsss[2] += "\n• Experiencing disturbing memories, nightmares, or\n  flashbacks about the traumatic experiences that make\n  the individual feel as if they are reliving the trauma";
        descsss[2] += "\n• Avoidance of situations, places, or even people that\n  remind the individual of the trauma they experienced";
        descsss[2] += "\n• Persistent feelings of isolation, hopelessness, or\n  difficulty experiencing positive emotions.";
        descsss[2] += "\n• Feeling constantly on edge, easily startled, irritable, or\n  having difficulty sleeping due to heightened anxiety and\n  alertness";
    }

    public void traverseDescPage()
    {
        if (isLeftArrow && currPage - 1 >= firstPage)
        {
            currPage--;


            if (currPage - 1 < firstPage)
            {
                leftArrow.SetActive(false);
                leftArrow.SetActive(false);
            }

            if (currPage + 1 <= lastPage)
            {
                rightArrow.SetActive(true);
                rightArrow.SetActive(true);
            }
        }

        if (!isLeftArrow && currPage + 1 <= lastPage)
        {
            currPage++;
            if (currPage + 1 > lastPage)
            {
                rightArrow.SetActive(false);
                rightArrow.SetActive(false);
            }

            if (currPage - 1 >= firstPage)
            {
                leftArrow.SetActive(true);
                leftArrow.SetActive(true);
            }
        }
        otherArrow.currPage = currPage;
        title.text = titles[currPage];
        desc.text = descsss[currPage];
    }
}
