package com.peanut.discord.equipment;

import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.peanut.discord.R;
import com.peanut.discord.models.Channel;

import java.util.ArrayList;
import java.util.List;

class ChannelAdapter extends RecyclerView.Adapter<ChannelAdapter.ChannelViewHolder> implements Inventory.ChannelListener {
    private List<Channel> listChannel;
    private ChannelAdapterListener channelAdapterListener;
    ChannelAdapter(ChannelAdapterListener channelAdapterListener){
        this.listChannel = new ArrayList<>();
        this.channelAdapterListener = channelAdapterListener;
    }

    @Override
    public void onAddListChannel(List<Channel> listChannel) {
        this.listChannel = listChannel;
        notifyDataSetChanged();
    }

    @Override
    public void onLeaveServer() {
        this.listChannel.clear();
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
        Channel channel = this.listChannel.get(i);
        TextView textView = channelViewHolder.textView;
        textView.setText(channel.getName());
        textView.setOnClickListener(v -> this.channelAdapterListener.onSelectChannel(channel));
    }

    @Override
    public int getItemCount() {
        return this.listChannel.size();
    }

    List<Channel> getListChannel() {
        return listChannel;
    }

    class ChannelViewHolder extends RecyclerView.ViewHolder{
        private TextView textView;
        private ChannelViewHolder(@NonNull View itemView){
            super(itemView);
            textView = itemView.findViewById(R.id.text_view_channel);
        }
    }


}