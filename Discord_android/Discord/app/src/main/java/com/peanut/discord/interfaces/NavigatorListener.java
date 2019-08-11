package com.peanut.discord.interfaces;

import com.peanut.discord.models.Channel;

public interface NavigatorListener {
    void onAddOrCreateServer();
    void onChannelChanged(Channel previousChannel, Channel currentChannel);

}
