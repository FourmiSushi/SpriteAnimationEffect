using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
// ReSharper disable UnusedMember.Global

namespace FourmiSushi.Animation
{
    /// <summary>
    /// エフェクトを再生するコンポーネント
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class AnimationEffect : MonoBehaviour
    {
        private PlayableGraph _graph;
        private AnimationMixerPlayable _mixer;
        private AnimationClipPlayable _currentPlayable;

        private void Awake()
        {
            _graph = PlayableGraph.Create();
            _mixer = AnimationMixerPlayable.Create(_graph, 1, true);
            AnimationPlayableOutput.Create(_graph, "output", GetComponent<Animator>()).SetSourcePlayable(_mixer);
            _graph.Play();
        }

        /// <summary>
        /// エフェクトを再生する
        /// </summary>
        /// <param name="animationClip">再生するアニメーション</param>
        /// <param name="loop">ループさせるかどうか</param>
        /// <returns>エフェクト終了時に終了するUniTask</returns>
        public UniTask Play(AnimationClip animationClip, bool loop = false)
        {
            if (_currentPlayable.IsValid())
                _currentPlayable.Destroy();
            _graph.Disconnect(_mixer, 0);
            _currentPlayable = AnimationClipPlayable.Create(_graph, animationClip);

            if (!loop)
            {
                _currentPlayable.SetDuration(animationClip.length);
            }

            _mixer.ConnectInput(0, _currentPlayable, 0);

            _mixer.SetInputWeight(0, 1);

            return UniTask.WaitUntil(() => _currentPlayable.IsDone(), PlayerLoopTiming.LastUpdate);
        }

        /// <summary>
        /// エフェクトを停止する
        /// </summary>
        public void Stop()
        {
            _currentPlayable.SetDone(true);
        }

        /// <summary>
        /// エフェクトをゲームオブジェクトに貼り付ける
        /// </summary>
        /// <param name="g">貼り付ける対象のゲームオブジェクト</param>
        /// <param name="offset">対象からずらす距離</param>
        public void AttachToGameObject(GameObject g, Vector2 offset = default)
        {
            Transform transform1;
            (transform1 = transform).SetParent(g.transform);
            transform1.localPosition = Vector3.zero + (Vector3)offset;
        }
    }
}