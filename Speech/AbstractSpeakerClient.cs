using System;
using HSCF.Communication.Scs.Communication.EndPoints.Tcp;
using HSCF.Communication.ScsServices.Client;

namespace HomeSeer.PluginSdk.Speech {

    public abstract class AbstractSpeakerClient : IFromSpeaker {
        
        protected const int SPEAKER_INTERFACE_VERSION = 10; // 10=first version for HS3 with new WCF like API
        
        protected readonly string ClientName;
        protected readonly string HostName;
        protected ISpeechAPI SpeechHost;
        protected string IpAddress { get; set; } = "127.0.0.1";
        
        private IScsServiceClient<ISpeechAPI> _client;
        
        private string _username = "";
        private string _password = "";

        public AbstractSpeakerClient(string pluginId, string name) {
            ClientName = name;
            HostName = pluginId;
        }

        public void Connect(string username, string password) {
            
            try {
                _client = ScsServiceClientBuilder.CreateClient<ISpeechAPI>(new ScsTcpEndPoint(IpAddress, 10401), this);
                _client.Disconnected += OnClientDisconnect;
                _client.Connected += OnClientConnect;
            }
            catch (Exception exception) {
                Console.WriteLine($"Error occured while initializing the speaker client : {exception.Message}");
                return;
            }
            
            Console.WriteLine($"Connecting speaker client {ClientName} to HomeSeer IP {IpAddress}");
            _username = username;
            _password = password;
            Connect(1);
        }
        
        private void Connect(int attempts) {
            
            try {
                _client.Connect();
                SpeechHost = _client.ServiceProxy;
                //make sure the interface is supported
                try {
                    var speakHostVersion = SpeechHost.version();
                    if (speakHostVersion != SPEAKER_INTERFACE_VERSION) {
                        Console.WriteLine("Speaker API version mismatch");
                        DisconnectNow();
                        return;
                    }
                }
                catch (Exception exception) {
                    Console.WriteLine($"Error attempting to connect to server, please check your connection options: {exception.Message}");
                    DisconnectNow();
                    return;
                }

                var response = SpeechHost.Connect(HostName, ClientName, IpAddress, _username, _password);

                if (string.IsNullOrEmpty(response)) {
                    return;
                }
                
                Console.WriteLine($"Error, Unable to connect speaker client interface: {response}");
                DisconnectNow();
            }
            catch (Exception exception) {
                Console.WriteLine($"Cannot connect speaker client attempt {attempts.ToString()}");
                if (exception.Message.ToLower().Contains("timeout occurred.") && attempts < 6) {
                    Connect(attempts + 1);
                }
                else {
                    DisconnectNow();
                    Console.WriteLine($"Error, Unable to connect speaker client interface: {exception.Message}");
                }
            }
        }

        protected virtual void OnClientDisconnect(object sender, EventArgs e) {
            Console.WriteLine($"Speaker client {ClientName} has disconnected");
        }

        protected virtual void OnClientConnect(object sender, EventArgs e) {
            Console.WriteLine($"Speaker client {ClientName} has connected");
        }

        private void DisconnectNow() {
            
            if (_client == null) {
                return;
            }
                
            try {
                SpeechHost = null;
                _client.Disconnect();
                _client.Disconnected -= OnClientDisconnect;
                _client.Connected    -= OnClientConnect;
                _client.Dispose();
            }
            catch (Exception exception) {
                Console.WriteLine($"Error while disconnecting speaker client : {exception.Message}");
            }
        }

        public bool Disconnect() {
            DisconnectNow();
            return true;
        }

        public abstract void SpeakText(string speakText, bool shouldWait);


        public abstract void PlayWaveFile(string fileName, int fileSize);

        public abstract bool IsBusy();

        public virtual void VRChanged() {
            //throw new NotImplementedException();
        }

        public abstract void StartListen();

        public abstract void StopListen();

        public virtual short SetVoice(string voice) {
            //throw new NotImplementedException();
            return 0;
        }

        public virtual void SpeakToFile(string speakText, string fileName, string voice = "") {
            //throw new NotImplementedException();
        }

        public virtual int SendMessage(string message, bool showBalloon) {
            //throw new NotImplementedException();
            return 0;
        }

        public virtual void PauseAudio() {
            //throw new NotImplementedException();
        }

        public virtual void UnPauseAudio() {
            //throw new NotImplementedException();
        }

        public virtual void MuteAudio() {
            //throw new NotImplementedException();
        }

        public virtual void UnMuteAudio() {
            //throw new NotImplementedException();
        }

        public virtual bool GetMuteStatus() {
            //throw new NotImplementedException();
            return false;
        }

        public virtual bool GetListenStatus() {
            //throw new NotImplementedException();
            return false;
        }

        public virtual short GetPauseStatus() {
            //throw new NotImplementedException();
            return 0;
        }

        public virtual string GetVoiceName() {
            //throw new NotImplementedException();
            return string.Empty;
        }

        public virtual int MEDIAPlay(string fileName, int fileSize) {
            //throw new NotImplementedException();
            return 0;
        }

        public virtual string MEDIAFunction(EMediaOperation operation, string p) {
            //throw new NotImplementedException();
            return string.Empty;
        }

        public virtual int GetVolume() {
            //throw new NotImplementedException();
            return 0;
        }

        public virtual void SetVolume(int vol) {
            //throw new NotImplementedException();
        }

        public abstract void StopSpeaking();

        public virtual int SetListenMode(int mode) {
            //throw new NotImplementedException();
            return 0;
        }

        public virtual int GetListenMode() {
            //throw new NotImplementedException();
            return 0;
        }

        public virtual void SetVRState(string state) {
            //throw new NotImplementedException();
        }

        public virtual void SetSpeaker(string speakerName) {
            //throw new NotImplementedException();
        }

        public virtual int SetSpeakingSpeed(int speed) {
            //throw new NotImplementedException();
            return 0;
        }

    }

}