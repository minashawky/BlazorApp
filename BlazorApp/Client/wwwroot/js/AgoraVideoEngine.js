const rtc = {
    // For the local client
    client: null,
    // For the local audio and video tracks
    localAudioTrack: null,
    localVideoTrack: null,
};

const options = {
    // Pass your app ID here
    appId: "87f7755baa194cfb8bc134a09b42157b",
    // Set the channel name
    channel: "demo_channel_name",
    // Pass a token if your project enables the App Certificate
    token: null,
};

async function startBasicCall() {

    rtc.client = AgoraRTC.createClient({ mode: "rtc", codec: "h264" });

    rtc.client.on("user-published", async (user, mediaType) => {
        // Subscribe to a remote user
        await rtc.client.subscribe(user);
        console.log("subscribe success");

        if (mediaType === "video" || mediaType === "all") {
            // Get `RemoteVideoTrack` in the `user` object.
            const remoteVideoTrack = user.videoTrack;
            // Dynamically create a container in the form of a DIV element for playing the remote video track.
            const mainContainer = document.getElementById("mainVideoContainer");
            const playerContainer = document.createElement("div");
            // Specify the ID of the DIV container. You can use the `uid` of the remote user.
            playerContainer.id = user.uid;
            playerContainer.style.width = "640px";
            playerContainer.style.height = "480px";
            mainContainer.appendChild(playerContainer);
            
            // Play the remote audio and video tracks
            // SDK dynamically creates a player in the container for playing the remote video track
            remoteVideoTrack.play(playerContainer);
        }

        if (mediaType === "audio" || mediaType === "all") {
            // Get `RemoteAudioTrack` in the `user` object.
            const remoteAudioTrack = user.audioTrack;
            // Play the audio track. Do not need to pass any DOM element
            remoteAudioTrack.play();
        }
    });

    rtc.client.on("user-unpublished", user => {
        // Get the dynamically created DIV container
        const playerContainer = document.getElementById(user.uid);
        // Destroy the container
        playerContainer.remove();
    });

    const uid = await rtc.client.join(options.appId, options.channel, options.token, null);

    // Create an audio track from the audio captured by a microphone
    rtc.localAudioTrack = await AgoraRTC.createMicrophoneAudioTrack();
    // Create a video track from the video captured by a camera
    rtc.localVideoTrack = await AgoraRTC.createCameraVideoTrack();
    // Publish the local audio and video tracks to the channel
    await rtc.client.publish([rtc.localAudioTrack, rtc.localVideoTrack]);

    console.log("publish success!");
}

async function leaveCall() {
    // Destroy the local audio and video tracks
    rtc.localAudioTrack.close();
    rtc.localVideoTrack.close();

    // Traverse all remote users
    rtc.client.remoteUsers.forEach(user => {
        // Destroy the dynamically created DIV container
        const playerContainer = document.getElementById(user.uid);
        playerContainer && playerContainer.remove();
    });

    // Leave the channel
    await rtc.client.leave();
}