package com.peanut.discord.equipment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.peanut.discord.R;
import com.peanut.discord.models.Channel;
import com.peanut.discord.models.Server;

import java.util.ArrayList;
import java.util.List;
public class ChannelAdapter extends RecyclerView.Adapter<ChannelAdapter.ChannelViewHolder> {
    private List<OnChannelButtonClickedListener> onChannelButtonClickedListeners;
    private List<OnChannelChangedListener> onChannelChangedListeners;
    public ChannelAdapter(){
    }
    public void establish(){
        onChannelButtonClickedListeners = new ArrayList<>();
        onChannelChangedListeners = new ArrayList<>();
    }
    public void tearDown(){
        onChannelButtonClickedListeners = null;
        onChannelChangedListeners = null;
    }
    void changeServer(Server previous, Server now){
        notifyDataSetChanged();
    }



    @NonNull
    @Override
    public ChannelViewHolder onCreateViewHolder(@NonNull ViewGroup viewGroup, int i) {
        LayoutInflater layoutInflater = LayoutInflater.from(viewGroup.getContext());
        View viewHolder = layoutInflater.inflate(R.layout.item_channel, viewGroup, false);
        return new ChannelViewHolder(viewHolder);
    }

    @Override
    public void onBindViewHolder(@NonNull ChannelViewHolder channelViewHolder, int i) {
        Channel channel = Inventory.getChannelsInCurrentServer().get(i);
        TextView textView = channelViewHolder.textView;
        textView.setText(channel.getChannelName());
//        textView.setOnClickListener(v -> this.channelAdapterListener.onSelectChannel(channel));
        textView.setOnClickListener(v -> {
            if(Inventory.currentChannel != channel){
                Channel previous = Inventory.currentChannel;
                Inventory.currentChannel = channel;
                for(OnChannelChangedListener listener : onChannelChangedListeners){
                    listener.onChannelChanged(previous, channel);
                }
            }
        });
    }

    @Override
    public int getItemCount() {
        if(Inventory.getChannelsInCurrentServer() == null){
            return 0;
        }
        return Inventory.getChannelsInCurrentServer().size();
    }


    class ChannelViewHolder extends RecyclerView.ViewHolder{
        private TextView textView;
        private ChannelViewHolder(@NonNull View itemView){
            super(itemView);
            textView = itemView.findViewById(R.id.text_view_channel);
        }
    }
    boolean registerOnChannelButtonClickedListener(OnChannelButtonClickedListener listener){
        if(listener == null){
            return false;
        }
        return onChannelButtonClickedListeners.add(listener);
    }
    boolean registerOnChannelChangedListener(OnChannelChangedListener listener){
        if(listener == null){
            return false;
        }
        return onChannelChangedListeners.add(listener);
    }
    boolean unregisterOnChannelButtonClickedListener(OnChannelButtonClickedListener listener){
        if(listener == null){
            return false;
        }
        return onChannelButtonClickedListeners.remove(listener);
    }
    boolean unregisterOnChannelChangedListener(OnChannelChangedListener listener){
        if(listener == null){
            return false;
        }
        return onChannelChangedListeners.remove(listener);
    }
    interface OnChannelButtonClickedListener{
        void onChannelButtonClicked(Channel channel);
    }
    interface OnChannelChangedListener{
        void onChannelChanged(Channel previous, Channel now);
    }
}