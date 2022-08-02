using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum OffMeshLinkMoveMethod
{
    Teleport,
    NormalSpeed,
    Parabola,
    Curve,
    NormalSpeedByCurve,
    NormalSpeedWithRotation
}

[RequireComponent(typeof(NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour
{
    public OffMeshLinkMoveMethod m_Method = OffMeshLinkMoveMethod.Parabola;
    public AnimationCurve m_Curve = new AnimationCurve();
    public float speedMultiplier = 1f;

    IEnumerator Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        while (true)
        {
            if (agent.isOnOffMeshLink)
            {
                if (m_Method == OffMeshLinkMoveMethod.NormalSpeed)
                    yield return StartCoroutine(NormalSpeed(agent));
                else if (m_Method == OffMeshLinkMoveMethod.Parabola)
                    yield return StartCoroutine(Parabola(agent, 2.0f, 0.5f));
                else if (m_Method == OffMeshLinkMoveMethod.Curve)
                    yield return StartCoroutine(Curve(agent, 0.5f));
                else if (m_Method == OffMeshLinkMoveMethod.NormalSpeedByCurve)
                    yield return StartCoroutine(NormalSpeedByCurve(agent));
                else if (m_Method == OffMeshLinkMoveMethod.NormalSpeedWithRotation)
                    yield return StartCoroutine(NormalSpeedWithRotation(agent));
                agent.CompleteOffMeshLink();
            }
            yield return null;
        }
    }

    IEnumerator NormalSpeed(NavMeshAgent agent)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        while (agent.transform.position != endPos)
        {
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime * speedMultiplier);
            yield return null;
        }
    }

    IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }

    IEnumerator Curve(NavMeshAgent agent, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (agent.transform.position != endPos)
        {
            float yOffset = m_Curve.Evaluate(normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }

    IEnumerator NormalSpeedWithRotation(NavMeshAgent agent)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        agent.updateRotation = false;
        Quaternion startRot = transform.rotation;
        //Vector3 startUpword = agent.transform.up;
        //Vector3 upwordOfLink = GetLocalUpword(startPos, endPos);
        //float multiplier = 1f;
        //if (Vector3.Dot(startUpword, upwordOfLink) < 0)
        //{
        //    multiplier = -1f;
        //}
        //Vector3 eulerAngles = agent.transform.localEulerAngles;
        //Vector3 targetEulers = eulerAngles + (90f * multiplier * Vector3.right);
        float t = 0f;
        float dur = Vector3.Distance(startPos, endPos) / (agent.speed * speedMultiplier);
        Vector3 targetForward = endPos - (startPos + agent.transform.forward * 0.3f);
        while (dur > 0f && t <= dur)
        {
            agent.transform.position = Vector3.Lerp(startPos, endPos, t / dur);
            transform.LookAt(targetForward + endPos);// = Quaternion.Lerp(startRot, Quaternion.LookRotation(targetForward.normalized), t / dur);

            //agent.transform.localEulerAngles = Vector3.Lerp(eulerAngles, targetEulers, t / dur);

            t += Time.deltaTime;
            yield return null;
        }
        agent.updateRotation = true;
    }

    IEnumerator NormalSpeedByCurve(NavMeshAgent agent)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        Vector3 upWord = GetLocalUpword(startPos, endPos);

        float duration = Vector3.Distance(startPos, endPos) / (agent.speed * speedMultiplier);
        float normalizedTime = 0.0f;

        while (agent.transform.position != endPos)
        {
            Vector3 offset = m_Curve.Evaluate(normalizedTime) * upWord;


            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + offset;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }

    private Vector3 GetLocalUpword(Vector3 startPos, Vector3 endPos)
    {
       // Debug.Log($"startPos.y = {startPos.y}, endPos.y= {endPos.y}, endPos.y-startPos.y = {endPos.y - startPos.y}");
        Vector3 linkDir = endPos - startPos;
        Vector3 right = Vector3.Cross(linkDir, Vector3.up).normalized;
        //return Vector3.Cross(right, linkDir).normalized;

        return Vector3.Cross(right, linkDir).normalized;
    }
}
