new Vue({
    el: '#app',
    data: {
        connection: null,
        userMessage: "",
        messages: [],
        receiving: false,
    },
    async mounted() {
        await this.connectToHub();
        this.initMessage();
    },
    methods: {
        connectToHub: async function () {
            const hubUrl = "/chat";
            this.connection = new signalR.HubConnectionBuilder().withUrl(hubUrl).build();
            this.connection.on("receiveMessage", this.receiveMessage);
            await this.connection.start();
        },
        initMessage: function () {
            this.sendMessage("こんにちは");
        },
        sendUserMessage: function () {
            if (this.receiving || !this.userMessage) return;
            this.messages.push({
                message: this.userMessage,
                role: "user",
                init: false,
            });
            this.sendMessage(this.userMessage);
        },
        sendMessage: function (message) {
            this.connection.invoke("sendMessage", message);
            this.userMessage = "";
            this.receiving = true;
            this.messages.push({
                message: "入力中...",
                role: "assistant"
            });
        },
        receiveMessage: function (resp) {
            const message = this.messages[this.messages.length - 1];

            if (!message.init) {
                message.init = true;
                message.message = "";
            }

            if (resp.finish) {
                this.receiving = false;
            } else {
                message.message += resp.content;
            }
        }
    }
});
