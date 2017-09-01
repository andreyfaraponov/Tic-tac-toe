using UnityEngine;

public class AI_control : MonoBehaviour
{
    int diff_level;
    Rounds_controller rd_control;
    GameObject buf;
    int iter;

    void Start()
    {
        iter = 5;
        diff_level = PlayerPrefs.GetInt("diff");
    }
    public void Ai_turn(Rounds_controller control, ref GameObject[,] field)
    {
        rd_control = control;
        if (diff_level == 2)
            hard_turn(ref field);
        if (diff_level == 1)
            medium_turn(ref field);
        if (diff_level == 0)
            light_turn(ref field);
    }

    void light_turn(ref GameObject[,] field)
    {
        bool turn;
        int x;
        int y;

        turn = false;
        while (!turn)
        {
            x = Random.Range(0, 3);
            y = Random.Range(0, 3);
            if (field[x, y].name == "empty(Clone)")
            {
                buf = Instantiate(rd_control.shape_init(), field[x, y].transform.position,
                    field[x, y].transform.rotation);
                buf.transform.SetParent(GameObject.Find("Field_control").transform);
                buf.transform.localScale = field[x, y].transform.localScale;
                DestroyImmediate(field[x, y]);
                field[x, y] = buf;
                turn = true;
            }
        }
    }

    void medium_turn(ref GameObject[,] field)
    {
        if (iter % 3 > 0)
        {
            light_turn(ref field);
        }
        else
            hard_turn(ref field);
        iter--;
    }

    void hard_turn(ref GameObject[,] field)
    {
        Vector3 point;
        Vector3 temp;
        string sh_name;

        point = new Vector3(-1, -1, 0);
        sh_name = rd_control.shape_init().name + "(Clone)";
        if (field[1, 1].name == "empty(Clone)")
            point = new Vector3(1, 1, 0);
        else if (field[2, 0].name != sh_name && field[2, 0].name != "empty(Clone)" && field[0, 2].name == "empty(Clone)")
            point = new Vector3(0, 2, 0);
        else if ((field[0, 2].name != sh_name && field[0, 2].name != "empty(Clone)") && field[2, 0].name == "empty(Clone)")
            point = new Vector3(2, 0, 0);
        else if (field[0, 0].name != sh_name && field[0, 0].name != "empty(Clone)" && field[2, 2].name == "empty(Clone)")
            point = new Vector3(2, 2, 0);
        else if ((field[2, 2].name != sh_name && field[2, 2].name != "empty(Clone)") && field[0, 0].name == "empty(Clone)")
            point = new Vector3(0, 0, 0);
        else if (point.x == -1 || point.y == -1)
        {
            temp = check_horizontal(field);
            if (temp.z == 2 && temp.x != -1 && temp.y != -1)
                point = temp;
            temp = check_vertical(field);
            if (temp.z == 2 && temp.x != -1 && temp.y != -1)
                point = temp;
            else
            {
                light_turn(ref field);
                return;
            }
        }

        buf = Instantiate(rd_control.shape_init(), field[(int)point.x, (int)point.y].transform.position,
                field[(int)point.x, (int)point.y].transform.rotation);
        buf.transform.SetParent(GameObject.Find("Field_control").transform);
        buf.transform.localScale = field[(int)point.x, (int)point.y].transform.localScale;
        DestroyImmediate(field[(int)point.x, (int)point.y]);
        field[(int)point.x, (int)point.y] = buf;
    }

    Vector3 check_horizontal(GameObject[,] field)
    {
        Vector3 result;
        GameObject[] temp;

        result = new Vector3(-1, -1, 0);
        for (int i = 0; i < 3; i++)
        {
            temp = new GameObject[3] { field[0, i], field[1, i], field[2, i] };
            if (enemy_count(temp) >= 2 && find_empty(temp) >= 0)
                result = new Vector3(find_empty(temp), i, 2);
        }
        return result;
    }

    Vector3 check_vertical(GameObject[,] field)
    {
        Vector3 result;
        GameObject[] temp;

        result = new Vector3(-1, -1, 0);
        for (int i = 0; i < 3; i++)
        {
            temp = new GameObject[3] { field[i, 0], field[i, 1], field[i, 2] };
            if (enemy_count(temp) == 2 && find_empty(temp) >= 0)
                result = new Vector3(i, find_empty(temp), 2);
        }
        return result;
    }

    int find_empty(GameObject[] field)
    {
        for (int i = 0; i < 3; i++)
        {
            if (field[i].name == "empty(Clone)")
                return i;
        }
        return (-1);
    }

    int enemy_count(GameObject[] field)
    {
        int res;

        res = 0;
        for (int i = 0; i < 3; i++)
        {
            if (field[i].name != rd_control.shape_init().name && field[i].name != "empty(Clone)")
                res++;
        }
        return res;
    }
}
