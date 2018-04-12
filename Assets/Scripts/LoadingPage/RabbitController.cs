using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RabbitController : MonoBehaviour
{

    private Vector3 _walkDestination,_runDestination;
    private bool _isWalking, _isRunning;

    // Use this for initialization
    void Start ()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(2);

        _isWalking = true;
	    this.GetComponent<Animator>().Play("rabbit_walk");
        _walkDestination = transform.position - Vector3.right * 92;
        _runDestination = _walkDestination - Vector3.right * 150;

        StartCoroutine(RabbitBehaviour());
    }

    private IEnumerator RabbitBehaviour()
    {
        yield return isWaiting();
        this.GetComponent<Animator>().Rebind();
        yield return WaitForRabbitToBeScared();
        GetComponent<AudioSource>().Play();
        yield return WaitForAnimationEnd();
        _isRunning = true;
    }
    private IEnumerator isWaiting()
    {
        do
        {
            yield return null;
        } while (_isWalking);
    }
    private IEnumerator WaitForAnimationEnd()
    {
        do
        {
            yield return null;
        } while (!this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("rabbit_walk_injured"));
    }
    private IEnumerator WaitForRabbitToBeScared()
    {
        do
        {
            yield return null;
        } while (!this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("rabbit_stand_afraid"));
    }
    // Update is called once per frame
    void Update () {
        if (transform.position != _walkDestination && _isWalking)
        {
            transform.position = Vector3.MoveTowards(transform.position, _walkDestination, 30 * Time.deltaTime);
        }
        else
        {
            _isWalking = false;
        }
        if (transform.position != _runDestination & _isRunning)
        {
            transform.position = Vector3.MoveTowards(transform.position, _runDestination, 50 * Time.deltaTime);
        }

    }
}
