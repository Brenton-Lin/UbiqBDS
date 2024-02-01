using System.Collections.Generic;
using System;
using Ubiq.Rooms;
using Ubiq.Messaging;
using Ubiq.Dictionaries;
using Ubiq.Spawning;
using Ubiq.Voip;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Ubiq.Avatars
{
    /// <summary>
    /// The AvatarManager creates and maintains Avatars for the local player and remote peers.
    /// AvatarManager operates using the RoomClient to create Avatar instances for all remote peers, though
    /// Avatars may be created outside of AvatarManager and maintained another way.
    /// </summary>
    public class CustomAvatarManager : MonoBehaviour
    {
        public PrefabCatalogue avatarCatalogue;
        public bool isVR = true;
        public Vector3 localCalibratedScale;
        public GameObject vrAvatarPrefab;
        public GameObject desktopAvatarPrefab;
        public GameObject selectedPrefab;
        public AvatarHints hints;

        /// <summary>
        /// The current avatar loaded for the local player. Be aware that this reference may change at any time
        /// if the underlying Prefab changes. Use the CreateLocalAvatar method to change the model (prefab).
        /// </summary>
        public Avatar LocalAvatar { get; private set; }
        public RoomClient RoomClient { get; private set; }

        public IEnumerable<Avatar> Avatars
        {
            get
            {
                return playerAvatars.Values;
            }
        }

        public class AvatarDestroyedEvent : UnityEvent<Avatar>
        {
        }

        public class AvatarCreatedEvent : ExistingListEvent<Avatar>
        {
        }

        /// <summary>
        /// Emitted after an Avatar is created for the first time.
        /// </summary>
        /// <remarks>
        /// This event may be emitted multiple times per peer, if the prefab changes and a Avatar/GameObject needs to be created.
        /// </remarks>
        public AvatarCreatedEvent OnAvatarCreated = new AvatarCreatedEvent();

        /// <summary>
        /// Emitted just before an Avatar is destroyed
        /// </summary>
        public AvatarDestroyedEvent OnAvatarDestroyed = new AvatarDestroyedEvent();

        private Dictionary<IPeer, Avatar> playerAvatars = new Dictionary<IPeer, Avatar>();
        private NetworkSpawner spawner;

        private GameObject spawnedPrefab;
        private GameObject spawned;

        private void Awake()
        {
            RoomClient = GetComponentInParent<RoomClient>();
            OnAvatarCreated.SetExisting(playerAvatars.Values);
        }

        private void Start()
        {
            spawner = new NetworkSpawner(NetworkScene.Find(this),
                RoomClient, avatarCatalogue, "ubiq.avatars.");
            spawner.OnSpawned += OnSpawned;
            spawner.OnDespawned += OnDespawned;

            RoomClient.OnPeerUpdated.AddListener(OnPeerUpdated);

            //UpdateLocalAvatar();
        }

        private void Update()
        {
            UpdateLocalAvatar();
        }

        private void OnDestroy()
        {
            RoomClient.OnPeerUpdated.RemoveListener(OnPeerUpdated);

            spawner.OnSpawned -= OnSpawned;
            spawner.OnDespawned -= OnDespawned;
            spawner.Dispose();
            spawner = null;
        }

        private void OnSpawned(GameObject gameObject, IRoom room,
            IPeer peer, NetworkSpawnOrigin origin)
        {
            var avatar = gameObject.GetComponentInChildren<Avatar>();
            avatar.SetPeer(peer);
            playerAvatars.Add(peer, avatar);
            //!!!!This is where the spawning function checks if the spawned avatar is for the room client, there should be a
            //preceding step where onspawned is called.
            if (peer == RoomClient.Me)
            {
                // Local setup
                if (LocalAvatar != null)
                {
                    // If we are changing the Avatar the LocalAvatar will not be destroyed until next frame so we can still get its transform.
                    gameObject.transform.localPosition = LocalAvatar.transform.localPosition;
                    gameObject.transform.localRotation = LocalAvatar.transform.localRotation;
                }
                avatar.IsLocal = true;

                if(isVR)
                {
                    //Set the scale for the local VR avatar. 
                    avatar.GetComponentInChildren<CustomThreePointAvatar>().avatarScale = localCalibratedScale;
                }

                avatar.SetHints(hints);
                gameObject.name = $"My Avatar #{peer.uuid}";

                LocalAvatar = avatar;
                //playerAvatars.Remove(peer);

            }
            else
            {
                // Remote setup
                avatar.gameObject.name = $"Remote Avatar #{peer.uuid}";
                //gameObject.transform.parent = transform;
                //OnAvatarCreated.Invoke(avatar);
            }
            //I changed this???
            gameObject.transform.parent = transform;
            OnAvatarCreated.Invoke(avatar);
        }

        private void OnDespawned(GameObject gameObject, IRoom room,
            IPeer peer)
        {
            var avatar = gameObject.GetComponentInChildren<Avatar>();
            OnAvatarDestroyed.Invoke(avatar);
            playerAvatars.Remove(avatar.Peer);
        }

        private void UpdateLocalAvatar()
        {
            // If we have an existing instance, but it is the wrong prefab, destroy it so we can start again
            if (spawnedPrefab && spawnedPrefab != selectedPrefab)
            {
                // Spawned with peer scope so callback is immediate
                spawner.Despawn(spawned);

                spawned = null;
                spawnedPrefab = null;
            }

            if (!spawnedPrefab)
            {
                //here we can set our logic to spawn a desktop or VR avatar.
                if (isVR)
                {
                    selectedPrefab = vrAvatarPrefab;


                }
                else
                {
                    selectedPrefab = desktopAvatarPrefab;
                }

                // Spawned with peer scope so callback is immediate
                spawned = spawner.SpawnWithPeerScope(selectedPrefab);
                spawnedPrefab = selectedPrefab;
            }

            // Avatars require a prefab. If missing, it means the remote player does not want an avatar.
            if (!selectedPrefab)
            {
                return;
            }

            // Create an instance of the correct prefab for this avatar

        }

        private void OnPeerUpdated(IPeer peer)
        {
            if (playerAvatars.TryGetValue(peer, out var avatar))
            {
                avatar.OnPeerUpdated.Invoke(peer);
            }
        }

        /// <summary>
        /// Find the AvatarManager for forest the Component is a member of. May return null if there is no AvatarManager for the scene.
        /// </summary>
        public static AvatarManager Find(MonoBehaviour Component)
        {
            var scene = NetworkScene.Find(Component);
            if (scene)
            {
                return scene.GetComponentInChildren<AvatarManager>();
            }
            return null;
        }
        //toggle VR flag for AvatarManager
        public void SetVRFlag(bool isVR)
        {
            this.isVR = isVR;
        }
        /// <summary>
        /// Finds the first avatar (if any) associated with the Peer
        /// </summary>
        public Avatar FindAvatar(IPeer Peer)
        {
            if (playerAvatars.ContainsKey(Peer))
            {
                return playerAvatars[Peer];
            }
            else
            {
                return null;
            }
        }
    }
}