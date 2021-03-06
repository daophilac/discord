package com.peanut.discord.equipment;

import android.graphics.Bitmap;
import android.os.Handler;
import android.os.Looper;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.peanut.androidlib.common.worker.UIWorker;
import com.peanut.discord.MainActivity;
import com.peanut.discord.R;
import com.peanut.discord.models.Message;
import com.peanut.discord.tools.ImageResolver;

import java.util.ArrayList;
import java.util.List;

class MessageAdapter extends RecyclerView.Adapter<MessageAdapter.MessageViewHolder> implements Inventory.MessageListener {
    private Handler handlerAddMessage;
    private List<Message> listMessage;
    private OnMessageLongClickListener onMessageLongClickListener;
    private UIWorker uiWorker;
    public MessageAdapter(){
        this.listMessage = new ArrayList<>();
        this.handlerAddMessage = new Handler(Looper.getMainLooper()){
            @Override
            public void handleMessage(android.os.Message msg) {
                notifyItemInserted(listMessage.size() - 1);
            }
        };
        uiWorker = new UIWorker();
    }

    @Override
    public void onAddListMessage(List<Message> listMessage) {
        this.listMessage = listMessage;
        notifyDataSetChanged();
    }

    @Override
    public void onLeaveServer() {
        this.listMessage.clear();
        notifyDataSetChanged();
    }

    @Override
    public void onAddMessage(Message message) {
        this.listMessage.add(message);
        this.handlerAddMessage.sendEmptyMessage(0);
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
        Message message = this.listMessage.get(i);
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
        return this.listMessage.size();
    }

    public List<Message> getListMessage() {
        return listMessage;
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