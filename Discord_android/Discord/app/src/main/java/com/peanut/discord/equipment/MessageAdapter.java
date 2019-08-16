package com.peanut.discord.equipment;
import android.graphics.Bitmap;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.peanut.androidlib.common.worker.UIWorker;
import com.peanut.discord.R;
import com.peanut.discord.customview.SendMessageButton;
import com.peanut.discord.models.Channel;
import com.peanut.discord.models.Message;
import com.peanut.discord.models.User;
import com.peanut.discord.tools.ImageResolver;

public class MessageAdapter extends RecyclerView.Adapter<MessageAdapter.MessageViewHolder> {
    private EditText editTextType;
    private SendMessageButton sendMessageButton;
    private OnMessageLongClickListener onMessageLongClickListener;
    private UIWorker uiWorker;
    public MessageAdapter(EditText editTextType, SendMessageButton sendMessageButton){
        this.editTextType = editTextType;
        this.sendMessageButton = sendMessageButton;
    }
    public void establish(){
        uiWorker = new UIWorker();
        sendMessageButton.setOnClickListener(v -> {
            if(Inventory.currentChannel == null){
                return;
            }
            String content = editTextType.getText().toString().trim();
            if(!content.equals("")){
                User currentUser = Inventory.currentUser;
                Channel currentChannel = Inventory.currentChannel;
                Message message = new Message(currentChannel.getChannelId(), currentUser.getUserId(), content);
                HubManager.Message.sendMessage(message);
            }
        });
    }
    public void tearDown(){
        uiWorker = null;
    }
    void changeChannel(Channel previous, Channel now){
        notifyDataSetChanged();
    }
    void addMessage(Message message){
        notifyItemInserted(Inventory.getMessagesInCurrentChannel().size() - 1);
        if(Inventory.currentUser.sameAs(message.getUser())){
            editTextType.setText("");
        }
    }

    @NonNull
    @Override
    public MessageViewHolder onCreateViewHolder(@NonNull ViewGroup viewGroup, int i) {
        LayoutInflater layoutInflater = LayoutInflater.from(viewGroup.getContext());
        View itemView = layoutInflater.inflate(R.layout.item_message, viewGroup, false);
        return new MessageViewHolder(itemView);
    }

    @Override
    public void onBindViewHolder(@NonNull MessageViewHolder messageViewHolder, int i) {
        Message message = Inventory.getMessagesInCurrentChannel().get(i);
        messageViewHolder.textViewMessage.setText(message.getUser().getUserName() + ": " + message.getContent());
        messageViewHolder.textViewTime.setText(message.getSimpleTime());
        ImageResolver.downloadUserImage(message.getUser().getImageName(), b -> {
            uiWorker.execute(() -> {
                Bitmap bitmap = Bitmap.createScaledBitmap(b, 128, 128, false);
                messageViewHolder.imageViewAvatar.setImageBitmap(bitmap);
            });
        });
        messageViewHolder.textViewMessage.setOnLongClickListener(v -> {
            if(onMessageLongClickListener != null){
                onMessageLongClickListener.onLongClick(message);
            }
            return true;
        });
    }
    public void setOnMessageLongClickListener(OnMessageLongClickListener onMessageLongClickListener) {
        this.onMessageLongClickListener = onMessageLongClickListener;
    }
    @Override
    public int getItemCount() {
        if(Inventory.getMessagesInCurrentChannel() == null){
            return 0;
        }
        return Inventory.getMessagesInCurrentChannel().size();
    }


    class MessageViewHolder extends RecyclerView.ViewHolder {
        private ImageView imageViewAvatar;
        private TextView textViewMessage;
        private TextView textViewTime;
        private MessageViewHolder(View itemView){
            super(itemView);
            this.imageViewAvatar = itemView.findViewById(R.id.image_view_avatar);
            this.textViewMessage = itemView.findViewById(R.id.text_view_message);
            this.textViewTime = itemView.findViewById(R.id.text_view_time);
        }
    }
    public interface OnMessageLongClickListener {
        void onLongClick(Message message);
    }
}