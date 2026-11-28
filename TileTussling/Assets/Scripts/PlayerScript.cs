using System.Collections;
using System.Collections.Generic;
using MapStuff;
using UnityEngine;

public class PlayerScript : TileItemScript
{
    public static GameObject player;
    public enum MoveAction {up, down, left, right, attackUp, attackDown, attackLeft, attackRight};
    [SerializeField] GameObject movementMarker;
    public List<MoveAction> path = new List<MoveAction>();
    public int maxMoves;
    [SerializeField] float moveSpeed;
    private bool executingMoves;

    [SerializeField] Color normalMarkerColor;
    [SerializeField] Color attackMarkerColor;
    private LineRenderer lineRender;

    void Awake()
    {
        player = gameObject;
        lineRender = movementMarker.GetComponent<LineRenderer>();
        ResetMovementPath();
    }

    // Update is called once per frame
    void Update()
    {
        if(!executingMoves)
        {
            if(Input.GetKeyDown(KeyCode.W))
                PlanMove(Vector2.up);
            if(Input.GetKeyDown(KeyCode.A))
                PlanMove(Vector2.left);
            if(Input.GetKeyDown(KeyCode.S))
                PlanMove(Vector2.down);
            if(Input.GetKeyDown(KeyCode.D))
                PlanMove(Vector2.right);
            
            if(Input.GetKeyDown(KeyCode.Return))
                StartCoroutine(ExecuteMoves());
        }
        
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
        else if(!IsAttackAction(path[path.Count - 1]))
        {
            if(path.Count < maxMoves)
                AddMoveAction(move);
            else
                AttemptAddingFinalAttackAction(move);
        }

        if(path.Count > 0)
        {
            if(IsAttackAction(path[path.Count - 1]))
                movementMarker.GetComponent<SpriteRenderer>().color = attackMarkerColor;
            else
                movementMarker.GetComponent<SpriteRenderer>().color = normalMarkerColor;
        }
    }

    public void AttemptAddingFinalAttackAction(Vector2 direction)
    {
        TileScript attemptedTile = GetTileAtEndOfPath();
        attemptedTile = attemptedTile.GetNeighborOfDirection(direction);

        if(attemptedTile.tileType != TileType.empty && attemptedTile.heldItem != null)
        {
            path.Add(GetAttackFromDirection(direction));
            movementMarker.transform.position = movementMarker.transform.position + (Vector3)direction;
            lineRender.positionCount++;
            lineRender.SetPosition(lineRender.positionCount - 1, lineRender.GetPosition(lineRender.positionCount - 2) + (Vector3)direction);
        }
    }

    public void AddMoveAction(Vector2 direction)
    {
        TileScript attemptedTile = GetTileAtEndOfPath();

        attemptedTile = attemptedTile.GetNeighborOfDirection(direction);

        if(attemptedTile.tileType != TileType.empty)
        {
            if(attemptedTile.heldItem == null)
            {
                path.Add(GetActionFromDirection(direction));
                movementMarker.transform.position = movementMarker.transform.position + (Vector3)direction;
                lineRender.positionCount++;
                lineRender.SetPosition(lineRender.positionCount - 1, lineRender.GetPosition(lineRender.positionCount - 2) + (Vector3)direction);
            }
            else
            {
                path.Add(GetAttackFromDirection(direction));
                movementMarker.transform.position = movementMarker.transform.position + (Vector3)direction;
                lineRender.positionCount++;
                lineRender.SetPosition(lineRender.positionCount - 1, lineRender.GetPosition(lineRender.positionCount - 2) + (Vector3)direction);
            }
        }
    }

    public TileScript GetTileAtEndOfPath()
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

        return attemptedTile;
    }

    public IEnumerator ExecuteMoves()
    {
        yield return null;

        if(!executingMoves)
        {
            executingMoves = true;
            movementMarker.SetActive(false);
            var lastWayPoint = transform.position;
            while(path.Count > 0)
            {
                if(!IsAttackAction(path[0]))
                {
                    transform.position = Vector2.MoveTowards(transform.position, (Vector2)lastWayPoint + GetDirectionOfAction(path[0]), moveSpeed * Time.deltaTime);

                    if(transform.position == lastWayPoint + (Vector3)GetDirectionOfAction(path[0]))
                    {
                        transform.parent.gameObject.GetComponent<TileScript>().PassHeldItemToDirection(GetDirectionOfAction(path[0]));
                        path.RemoveAt(0);
                        lastWayPoint = transform.position;
                    }
                    yield return null;
                }
                else
                {
                    transform.parent.gameObject.GetComponent<TileScript>().GetNeighborOfDirection(GetDirectionOfAction(path[0])).DamageHeldItem();
                    Debug.Log("Did an attack >:D");
                    path.RemoveAt(0);
                }
            }

            executingMoves = false;
            ResetMovementPath();
            movementMarker.SetActive(true);
        }
        
    }

    public void ResetMovementPath()
    {
        lineRender.positionCount = 1;
        lineRender.SetPosition(0, transform.position);
        movementMarker.transform.position = transform.position;
    }

    public bool CheckDirectionForMovement(Vector2 direction)
    {
        return true;
    }

    public bool IsAttackAction(MoveAction action)
    {
        return (action == MoveAction.attackUp || action == MoveAction.attackDown || action == MoveAction.attackLeft || action == MoveAction.attackRight);
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
    public MoveAction GetAttackFromDirection(Vector2 direction)
    {
        if(direction == Vector2.up)
            return MoveAction.attackUp;
        if(direction == Vector2.down)
            return MoveAction.attackDown;
        if(direction == Vector2.left)
            return MoveAction.attackLeft;
        if(direction == Vector2.right)
            return MoveAction.attackRight;
        
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
