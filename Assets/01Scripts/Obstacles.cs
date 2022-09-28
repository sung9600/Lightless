using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField]
    protected int HP = 1;
    [SerializeField]
    protected int MaxHp = 1;
    [SerializeField]
    protected List<int> Positions = new List<int>();

    public Obstacles(int MaxHp, List<int> Positions)
    {
        this.HP = this.MaxHp = MaxHp;
        this.Positions = Positions;
    }

    public bool ContainPosition(int Position)
    {
        return Positions.Contains(Position);
    }

    public void GetDamage(int Damage)
    {
        if (Damage < 0) return;

        HP -= Damage;
        if (HP <= 0)
        {
            Debug.Log($"Deleted Obstacle at Position{Positions[0]}");

            //mapmanager에서 타일 수정
            MapManager.GetInstance().DestroyObstacleTile(Positions);

            StageManager.GetInstance().GetObstacles().Remove(this);
        }
    }


}
