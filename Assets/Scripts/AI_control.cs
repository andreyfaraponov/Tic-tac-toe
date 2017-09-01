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

        point = new Vector3(-1, -1, 0);
        temp = check_crosslines(field);
        if (temp.z > point.z && temp.x != -1 && temp.y != -1)
            point = temp;
        temp = check_horizontal(field);
        if (temp.z > point.z && temp.x != -1 && temp.y != -1)
            point = temp;
        temp = check_vertical(field);
        if (temp.z > point.z && temp.x != -1 && temp.y != -1)
            point = temp;

        if (field[1, 1].name == "empty(Clone)")
            point = new Vector3(1, 1, 0);
        else if (field[0, 2].name == "empty(Clone)")
            point = new Vector3(0, 2, 0);
        Debug.Log(point.x + " " + point.y);
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
            for (int z = 1; z <= 2; z++)
                if (enemy_count(temp) > z && find_empty(temp) >= 0)
                    result = new Vector3(find_empty(temp), i, z);
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
            for (int z = 1; z <= 2; z++)
                if (enemy_count(temp) >= z && find_empty(temp) >= 0)
                    result = new Vector3(i, find_empty(temp), z);
        }
        return result;
    }

    Vector3 check_crosslines(GameObject[,] field)
    {
        Vector3 result;
        GameObject[] temp;
        int o;
        int q;

        o = 0;
        q = 0;
        result = new Vector3(-1, -1, 0);
        for (int i = 0; i < 3; i++)
        {
            temp = new GameObject[3] { field[0, 0], field[1, 1], field[2, 2] };
            if (field[i, i].name != rd_control.shape_init().name && field[i, i].name != "empty(Clone)")
                o++;
            if (find_empty(temp) > -1 && o != 3)
                result = new Vector3(find_empty(temp), find_empty(temp), o);
        }
        for (int i = 0; i < 3 && o < 3; i++)
        {
            for (int j = 2; j >= 0; j--)
            {
                temp = new GameObject[3] { field[0, 2], field[1, 1], field[2, 0] };
                Debug.Log(field[i, j].name + "x = " + i + "; y = " + j);
                if (field[i, j].name != rd_control.shape_init().name && field[i, j].name != "empty(Clone)"
                    && q < 3)
                    q++;
                else
                    return result;
                if (q > 0 && q > o && q != 3)
                {
                    if (find_empty(temp) == 0)
                        result = new Vector3(0, 2, q);
                    else if (find_empty(temp) == 1)
                        result = new Vector3(1, 1, q);
                    else if (find_empty(temp) == 2)
                        result = new Vector3(2, 0, q);
                }
            }
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
