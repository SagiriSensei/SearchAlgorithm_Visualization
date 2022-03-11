using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager: MonoBehaviour
{
    public Button chooseEndBtn;
    public Button chooseBlocksBtn;
    public Button runBtn;
    public Button restartBtn;
    public Button astarBtn;
    public Button bfsBtn;
    public Button quit;
    public TMP_Text tips;
    //public bool UIenabled = true;

    private void Awake()
    {
        chooseEndBtn = transform.GetChild(0).GetComponent<Button>();
        astarBtn = transform.GetChild(1).GetComponent<Button>();
        bfsBtn = transform.GetChild(2).GetComponent<Button>();
        quit = transform.GetChild(3).GetComponent<Button>();
        tips = transform.GetChild(4).GetChild(0).GetComponent<TMP_Text>();
        chooseBlocksBtn = transform.GetChild(5).GetComponent<Button>();
        runBtn = transform.GetChild(6).GetComponent<Button>();
        restartBtn = transform.GetChild(7).GetComponent<Button>();
    }

    private void Start()
    {
        chooseEndBtn.onClick.AddListener(ChooseEnd);
        chooseBlocksBtn.onClick.AddListener(ChooseBlocks);
        runBtn.onClick.AddListener(Running);
        restartBtn.onClick.AddListener(Restart);
        tips.text = "Choose a start      (1 / 3)";
    }

    private void Update()
    {
        if (gameManager.instance.algoCate == gameManager.AlgorithmCategory.Astar)
        {
            astarBtn.Select();
        }
        else if (gameManager.instance.algoCate == gameManager.AlgorithmCategory.BFS)
        {
            bfsBtn.Select();
        }
    }

    void ChooseEnd()
    {
        if (gameManager.instance.state == gameManager.GameStates.Start)
        {
            //动画是否结束播放且选择了开始方块
            if (!gameManager.instance.startIdx.IsNull() && CubeAnim.animEnd)
            {
                gameManager.instance.state = gameManager.GameStates.End;
                gameManager.instance.vis[gameManager.instance.startIdx.x][gameManager.instance.startIdx.y] = 1;
                chooseEndBtn.gameObject.SetActive(false);
                chooseBlocksBtn.gameObject.SetActive(true);
                tips.text = "Choose an end      (2 / 3)";
            }
        }
    }

    void ChooseBlocks()
    {
        if (gameManager.instance.state == gameManager.GameStates.End)
        {
            //动画是否播放完且选择了结束方块
            if (!gameManager.instance.endIdx.IsNull() && CubeAnim.animEnd)
            {
                gameManager.instance.state = gameManager.GameStates.Block;
                chooseBlocksBtn.gameObject.SetActive(false);
                runBtn.gameObject.SetActive(true);
                tips.text = "Choose blocks      (3 / 3)";
            }
        }
    }

    void Running()
    {
        if (gameManager.instance.state == gameManager.GameStates.Block)
        {
            //动画是否播放完
            if (CubeAnim.animEnd)
            {
                gameManager.instance.state = gameManager.GameStates.Running;
                //开始寻路
                gameManager.instance.SearchWay();
                runBtn.gameObject.SetActive(false);
                restartBtn.gameObject.SetActive(true);
                tips.text = "Running";
            }
        }
    }

    void Restart()
    {
        if (gameManager.instance.state == gameManager.GameStates.Running)
        {
            //动画是否播放完
            if (CubeAnim.animEnd)
            {
                gameManager.instance.state = gameManager.GameStates.Start;
                //重新开始游戏
                gameManager.instance.Initialize();
                restartBtn.gameObject.SetActive(false);
                chooseEndBtn.gameObject.SetActive(true);
                tips.text = "Choose a start      (1 / 3)";
            }
        }
    }

    public void ChooseAstar()
    {
        gameManager.instance.algoCate = gameManager.AlgorithmCategory.Astar;
    }

    public void ChooseBFS()
    {
        gameManager.instance.algoCate = gameManager.AlgorithmCategory.BFS;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
