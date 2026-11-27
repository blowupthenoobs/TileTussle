using System.Collections;
using System.Collections.Generic;
using MapStuff;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    
    public enum MoveAction {up, down, left, right, attackUp, attackDown, attackLeft, attackRight};
    [SerializeField] GameObject movementMarker;
    public List<MoveAction> path = new List<MoveAction>();
    public int maxMoves;

    private LineRenderer lineRender;

    void Awake()
    {
        lineRender = movementMarker.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
            PlanMove(Vector2.up);
        if(Input.GetKeyDown(KeyCode.A))
            PlanMove(Vector2.left);
        if(Input.GetKeyDown(KeyCode.S))
            PlanMove(Vector2.down);
        if(Input.GetKeyDown(KeyCode.D))
            PlanMove(Vector2.right);
    }

    public void PlanMove(Vector2 move)
    {
        if(path.Count == 0)
        {
            AddMoveAction(move);
        }
        else if(move == -1 * GetDirectionOfAction(path[path.Count -1]))
        {
            path.RemoveAt(path.Count -1);
            lineRender.positionCount--;
            movementMarker.transform.position = movementMarker.transform.position + (Vector3)move;
        }
        else if(path.Count < maxMoves)
        {
            AddMoveAction(move);
        }
    }

    public void AddMoveAction(Vector2 direction)
    {
        var parentTile = transform.parent.gameObject.GetComponent<TileScript>();
        TileScript attemptedTile = parentTile;

        if(path.Count != 0)
        {
            for(int i = 0; i < path.Count; i++)
            {
                attemptedTile = attemptedTile.GetNeighborOfDirection(GetDirectionOfAction(path[i]));
            }
        }

       attemptedTile = attemptedTile.GetNeighborOfDirection(direction);

        if(attemptedTile.tileType != TileType.empty && attemptedTile.heldItem == null)
        {
            // Debug.Log(attemptedTile.tileType);
            // Debug.Log(attemptedTile.heldItem);
            path.Add(GetActionFromDirection(direction));
            movementMarker.transform.position = movementMarker.transform.position + (Vector3)direction;
        }
    }

    public bool CheckDirectionForMovement(Vector2 direction)
    {
        return true;
    }

    public MoveAction GetActionFromDirection(Vector2 direction)
    {
        if(direction == Vector2.up)
            return MoveAction.up;
        if(direction == Vector2.down)
            return MoveAction.down;
        if(direction == Vector2.left)
            return MoveAction.left;
        if(direction == Vector2.right)
            return MoveAction.right;
        
        Debug.Log("no listed action for following direction");
        Debug.Log(direction);
        return MoveAction.up;
    }

    public Vector2 GetDirectionOfAction(MoveAction action)
    {
        if(action == MoveAction.up || action == MoveAction.attackUp)
            return Vector2.up;
        if(action == MoveAction.down || action == MoveAction.attackDown)
            return Vector2.down;
        if(action == MoveAction.left || action == MoveAction.attackLeft)
            return Vector2.left;
        if(action == MoveAction.right || action == MoveAction.attackRight)
            return Vector2.right;
        
        Debug.Log("unlisted action: " + action);
        return Vector2.zero;
    }
}
