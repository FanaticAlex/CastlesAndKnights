using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class SoundEffectsPlayer: MonoBehaviour
    {
        public AudioClip Click;
        public AudioClip MenuChange;
        public AudioClip PutCard;
        public AudioSource AudioSource;


        void Start()
        {
            if (AudioSource == null)
                AudioSource = FindAnyObjectByType<AudioSource>();

            AudioSource.loop = false;
        }

        public void PlayClick()
        {
            AudioSource.clip = Click;
            AudioSource.Play();
        }

        public void PlayMenuChange()
        {
            AudioSource.clip = MenuChange;
            AudioSource.Play();
        }

        public void PlayPutCard()
        {
            AudioSource.clip = PutCard;
            AudioSource.Play();
        }
    }
}
