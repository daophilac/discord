package com.peanut.discord.models;
import java.util.List;
public class ChannelPermission {
    private int permissionId;
    private String permissionName;
    private String description;
    private List<ChannelLevelPermission> channelLevelPermissions;
    public int getPermissionId() {
        return permissionId;
    }
    public void setPermissionId(int permissionId) {
        this.permissionId = permissionId;
    }
    public String getPermissionName() {
        return permissionName;
    }
    public void setPermissionName(String permissionName) {
        this.permissionName = permissionName;
    }
    public String getDescription() {
        return description;
    }
    public void setDescription(String description) {
        this.description = description;
    }
    public List<ChannelLevelPermission> getChannelLevelPermissions() {
        return channelLevelPermissions;
    }
    public void setChannelLevelPermissions(List<ChannelLevelPermission> channelLevelPermissions) {
        this.channelLevelPermissions = channelLevelPermissions;
    }
}
